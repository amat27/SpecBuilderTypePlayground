using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SpecBuilderTypePlayground.Spec
{
    internal class Specification<TFrom, TTo> : ISpecification<TFrom, TTo>
    {        
        public Expression<Func<TFrom, TTo>> Mapper { get; set; } = null!;
        public Expression<Func<TFrom, bool>> Predicate { get; set; } = null!;

        public IQueryable<TTo> Apply(IQueryable<TFrom> query) => query.Where(this.Predicate).Select(this.Mapper);

        public IQueryable Apply(IQueryable query)
        {
            return this.Apply((IQueryable<TFrom>)query);
        }
    }

    internal interface ISpecification<in TFrom, out TTo>: ISpecification
    {
        IQueryable<TTo> Apply(IQueryable<TFrom> query);
    }

    internal interface ISpecification 
    { 
        IQueryable Apply(IQueryable query);
    }
}
