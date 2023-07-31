using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecBuilderTypePlayground.Spec
{
    internal static class SpecBuilderExtensions
    {
        public static SpecBuilder<TFrom, TTo> BuildBatchTaskSpec<TFrom, TTo>(this SpecBuilder<TFrom, TTo> specBuilder)
        {
            return specBuilder;
        }
    }
}
