using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SpecBuilderTypePlayground.Spec
{
    internal class SpecBuilder<TFrom, TTo>
    {
        internal Specification<TFrom, TTo> Build() => new Specification<TFrom, TTo>();
    }
}
