using System.Collections;
using SpecBuilderTypePlayground.Context;
using SpecBuilderTypePlayground.Spec;

namespace SpecBuilderTypePlayground
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            // regular query
            var res = HaveAsync(b => b.QueryActiveTask());
            var res2 = GetAsync(b => b.QueryActiveTask().ByClusterId("cluster-1").Map(t => t.ActiveProperty));

            // parsing query properties
            IEnumerable<Target> targets = new List<Target>();
            var queryToSpec = new QueryBagToSpecParser();

            foreach (var target in targets)
            {
                var parser = target switch
                {
                    Target.All => queryToSpec.ByClusterId("cluster-1"),
                    Target.Active => queryToSpec.Active().ByClusterId("cluster-1"),
                    Target.Finalized => queryToSpec.Finalized().ByClusterId("cluster-1"),
                };  
            }

            var spec = queryToSpec.Build();
            var res3 = spec switch
            { 
                ISpecification<ActiveTask, TaskInternalState> s => GetAsync(s),
                ISpecification<FinalizedTask, TaskInternalState> s => GetAsync(s),
                _ => throw new NotImplementedException()
            };
        }

        private static bool HaveAsync<T>(Func<BatchTaskSpecBuilder, ISpecBuilder<T, T>> building) {
            return true;
        }

        private static TTo GetAsync<TFrom, TTo>(Func<BatchTaskSpecBuilder, ISpecBuilder<TFrom, TTo>> building) {
            var builder = building(new BatchTaskSpecBuilder());
            return GetAsync(builder.Build());
        }

        private static TTo GetAsync<TFrom, TTo>(ISpecification<TFrom, TTo> spec)
        {
            // repository.GetAsync(spec, target);
            return default!;
        }

        enum Target { Active, Finalized, All }
        
    }
}