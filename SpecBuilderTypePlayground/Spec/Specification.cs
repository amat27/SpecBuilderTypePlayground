using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SpecBuilderTypePlayground.Spec
{
    internal class Specification<TFrom, TTo> : ISpecification
    {        public Expression<Func<TFrom, TTo>> Mapper { get; set; } = null!;

    }

    internal interface ISpecification
    {
    }
}
