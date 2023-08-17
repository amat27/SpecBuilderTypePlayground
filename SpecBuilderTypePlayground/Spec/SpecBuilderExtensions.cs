using SpecBuilderTypePlayground.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecBuilderTypePlayground.Spec
{
    internal static class SpecBuilderExtensions
    {
        public static SpecBuilder<BatchTask, TTo> BuildBatchTaskSpec<TTo>(this SpecBuilder<BatchTask, TTo> specBuilder)
        {
            return specBuilder;
        }
    }
}
