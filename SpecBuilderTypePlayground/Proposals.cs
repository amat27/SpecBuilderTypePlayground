using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecBuilderTypePlayground
{
    internal class Proposals
    {
        /*
         * - Option 1:
         * public Task<bool> BatchTasks.HaveAsync(Func<TaskSpecBuilder, ISpecification> building);
         * Where Specification<BatchTask> : ISpecification. i.e., Specification will be the base class for all generic type, with probably no members.
         * 
         * Full example for task query will be:
         * BatchTasks.HaveAsync(builder => builder.ActiveTasks() // explicitly replaced by a ActiveTaskSpecBuilder
         *                                        .QueuedTasks().IncludePendingReason().Build());
         * 
         * And we can provide syntax sugar like
         * BatchTasks.HaveAsync(builder => builder.QueuedTasks() // implicitly replaced by a ActiveTaskSpecBuilder
         *                                        .IncludePendingReason().Build());
         *                                    
         * 
         * - Option 2:
         * public Task<bool> BatchTasks.HaveAsync(Func<TaskSpecBuilder              // covariant here
         *                                        , ISpecification<BatchTask>>      // contravariant here
         *                                        );
         *                                        
         * Here, `building` returns a specification for BatchTasks child type. We do runtime dispatching, but no type mismatch.
         * The problem is ISpecification<out T> will also be an empty interface - because TFrom in the implementation is always
         * 
         * - Option 3:
         * If we have ISpecification<out T> and ISpecBuilder<out T> which can be instantiated to:
         *  ISpecBuilder<BatchTask>
         *  ISpecBuilder<ActiveTask>
         *  ISpecBuilder<FinalizedTask>
         *  
         * And then we can use Extension methods to add more methods to ISpecification<out T>:
         *  public ISpecification<T> Build<T>(this ISpecBuilder<BatchTask> spec) where T : BatchTask
         *  
         * - Option 4:
         * Covariant on the delegates!
         * No - Expressions can not covariant.
         * 
         */
    }
}
