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
        public Expression<Func<ActiveTask, bool>> Predicate { get; set; } = null!;

        public ActiveTaskSpecBuilder QueryActiveTask() => new ActiveTaskSpecBuilder();

        public FinalizedTaskSpecBuilder QueryFinalizedTask() => new FinalizedTaskSpecBuilder();

        public AllTaskSpecBuilder QueryAllTask() => new AllTaskSpecBuilder();

        public BatchTaskSpecBuilder ByClusterId(string clusterId) { this.Predicate = t => t.SharedProperty == clusterId; return this; }

        public SpecBuilder<ActiveTask, ActiveTask> ToSpecBuilder() => new() { Predicate = this.Predicate };
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

    internal class QueryBagToSpecParser
    {
        AllTaskSpecBuilder allTaskSpecBuilder = new AllTaskSpecBuilder();
        ActiveTaskSpecBuilder activeTaskSpecBuilder = new ActiveTaskSpecBuilder();
        FinalizedTaskSpecBuilder finalizedTaskSpecBuilder = new FinalizedTaskSpecBuilder();

        private State state = State.All;

        enum State { Active, Finalized, All, Invalid }

        public QueryBagToSpecParser ByClusterId(string clusterId) 
        { 
            // add filter according to state
            return this; 
        }

        public QueryBagToSpecParser Active()
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
        public QueryBagToSpecParser Finalized()
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
                State.Active => this.activeTaskSpecBuilder.Build(),
                State.Finalized => this.finalizedTaskSpecBuilder.Build(),
                State.All => this.allTaskSpecBuilder.Build(),
            };
        }
    }
}
