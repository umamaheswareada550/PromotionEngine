using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PromotionEngine.Specifications
{
    public class NskuForFixedPriceSpecification : Specification<Product>
    {
        public NskuForFixedPriceSpecification()
        {

        }

        public override Expression<Func<Product, double>> ToExpression()
        {
            throw new NotImplementedException();
        }
    }
}
