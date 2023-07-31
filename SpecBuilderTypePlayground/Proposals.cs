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
         * public Task<bool> BatchTasks.HaveAsync(Func<TaskSpecBuilder, Specification> building);
         * Where Specification<BatchTask> : Specification. i.e., Specification will be the base class for all generic type, with probably no members.
         * And for SpecBuilder, we will have SpecBuilder<T1, T2> : SpecBuilder, and `SpecBuilder.Build() -> Specification`
         * 
         * Here, `building` returns the very basic type. And it relies on run time check for any type mismatch.
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
         * 
         */
    }
}
