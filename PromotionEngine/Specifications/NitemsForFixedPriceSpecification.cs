using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PromotionEngine.Specifications
{
    public class NitemsForFixedPriceSpecification : Specification<Product>
    {
        protected List<Promotion> _promotions;
        protected IDictionary<string, int> _basePrices;
        private readonly Product _product;

        public NitemsForFixedPriceSpecification(Product product, List<Promotion> promotions, IDictionary<string, int> basePrices)
        {
            _product = product;
            _promotions = promotions;
            _basePrices = basePrices;
        }

        public override Expression<Func<Product, double>> ToExpression()
        {
            var promotion = _promotions.Where(p => p.SkuId == _product.Key).FirstOrDefault();
            switch (_product.Quantity % promotion.Quantity)
            {
                case 0: _product.Price = _basePrices[_product.Key] * _product.Quantity - promotion.DiscountPrice; break;
                default: _product.Price = _basePrices[_product.Key] * _product.Quantity; break;
            }

            return _product => _product.Price;
        }
    }
}
