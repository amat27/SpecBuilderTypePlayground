using SpecBuilderTypePlayground.Context;
using SpecBuilderTypePlayground.Spec;

namespace SpecBuilderTypePlayground
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            SpecBuilder<BatchTask, object> batchTaskSpecBuilder = new();
            SpecBuilder<ActiveTask, object> activeTaskSpecBuilder = new();

            var f = (ActiveTask at) => true;
            Specification<ActiveTask, bool> atSpec = new() { Mapper = (ActiveTask at) => true };

            HaveAsync(b => b.Build());


        }

        private static bool HaveAsync(Func<SpecBuilder<BatchTask, BatchTask>, ISpecification> building) {
            return true;
        }
    }
}