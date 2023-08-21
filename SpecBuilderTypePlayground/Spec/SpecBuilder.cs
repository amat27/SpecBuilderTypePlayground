using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using SpecBuilderTypePlayground.Context;

namespace SpecBuilderTypePlayground.Spec
{
    internal interface ISpecBuilder<in TFrom, out TTo> : ISpecBuilder
    {
        new ISpecification<TFrom, TTo> Build();
    }

    internal interface ISpecBuilder
    {
        ISpecification Build();
    }

    internal class SpecBuilder<TFrom, TTo> : ISpecBuilder<TFrom, TTo>
    {
        public Expression<Func<TFrom, TTo>> Mapper { get; set; } = null!;
        public Expression<Func<TFrom, bool>> Predicate { get; set; } = null!;

        public ISpecification<TFrom, TTo> Build() => new Specification<TFrom, TTo>() { Mapper = this.Mapper, Predicate = this.Predicate };

        public SpecBuilder<TFrom, TNewTo> Map<TNewTo>(Expression<Func<TFrom, TNewTo>> mapper)
        {
            return new SpecBuilder<TFrom, TNewTo>() { Mapper = mapper, Predicate = this.Predicate };
        }

        ISpecification ISpecBuilder.Build()
        {
            return Build();
        }
    }

    internal class BatchTaskSpecBuilder
    {
        public ActiveTaskSpecBuilder QueryActiveTask() => new ActiveTaskSpecBuilder();

        public FinalizedTaskSpecBuilder QueryFinalizedTask() => new FinalizedTaskSpecBuilder();

        public AllTaskSpecBuilder QueryAllTask() => new AllTaskSpecBuilder();
    }

    internal class AllTaskSpecBuilder : SpecBuilder<BatchTask, BatchTask>
    {
        public AllTaskSpecBuilder ByClusterId(string clusterId) { this.Predicate = t => t.SharedProperty == clusterId; return this; }

    }

    internal class ActiveTaskSpecBuilder : SpecBuilder<ActiveTask, ActiveTask>
    {
        public ActiveTaskSpecBuilder ByClusterId(string clusterId) { this.Predicate = t => t.ActiveProperty == clusterId; return this; }


    }

    internal class FinalizedTaskSpecBuilder:SpecBuilder<FinalizedTask, FinalizedTask>
    {
        public FinalizedTaskSpecBuilder ByClusterId(string clusterId) { this.Predicate = t => t.FinalizedProperty == clusterId; return this; }
    }

    internal class QueryBagToTaskInternalSpecParser : QueryBagToSpecParser<TaskInternalState> {
        QueryBagToTaskInternalSpecParser() {
            // batchTaskToInternalState = ...;
            // activeTaskToInternalState = ...;
            // finalizedTaskToInternalState = ...;
        }
    }

    internal class QueryBagToSpecParser<T>
    {
        AllTaskSpecBuilder allTaskSpecBuilder = new AllTaskSpecBuilder();
        ActiveTaskSpecBuilder activeTaskSpecBuilder = new ActiveTaskSpecBuilder();
        FinalizedTaskSpecBuilder finalizedTaskSpecBuilder = new FinalizedTaskSpecBuilder();

        protected Expression<Func<BatchTask, T>> batchTaskToInternalState = null!;
        protected Expression<Func<ActiveTask, T>>  activeTaskToInternalState = null!;
        protected Expression<Func<FinalizedTask, T>> finalizedTaskToInternalState = null!;

        private State state = State.All;

        enum State { Active, Finalized, All, Invalid }

        public QueryBagToSpecParser<T> ByClusterId(string clusterId) 
        { 
            // add filter according to state
            return this; 
        }

        public QueryBagToSpecParser<T> Active()
        {
            if (this.state == State.All)
            {
                this.state = State.Active;
                return this;

            }
            else
            {
                this.state = State.Invalid;
                throw new InvalidOperationException();
            }

        }
        public QueryBagToSpecParser<T> Finalized()
        {
            if (this.state == State.All)
            { this.state = State.Finalized; return this; }
            else
            {
                this.state = State.Invalid;
                throw new InvalidOperationException();
            }

        }

        public ISpecification Build() 
        {
            return this.state switch
            {
                State.Active => this.activeTaskSpecBuilder.Map(this.activeTaskToInternalState).Build(),
                State.Finalized => this.finalizedTaskSpecBuilder.Map(this.finalizedTaskToInternalState).Build(),
                State.All => this.allTaskSpecBuilder.Map(this.batchTaskToInternalState).Build(),
            };
        }
    }
}
