using SpecBuilderTypePlayground.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public static SpecBuilder<TFrom, TTo> Map<TFrom, TTo>(this SpecBuilder<TFrom, TFrom> specBuilder, Expression<Func<TFrom, TTo>> mapper)
        {
            return new SpecBuilder<TFrom, TTo>() { Mapper = mapper, Predicate = specBuilder.Predicate };
        }

    }
}
