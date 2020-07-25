using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PromotionEngine.Specifications
{
    public class NskuForFixedPriceSpecification : Specification<Product>
    {
        protected List<Promotion> _promotions;
        protected IDictionary<string, int> _basePrices;
        private readonly Product _product;

        public NskuForFixedPriceSpecification()
        {
        }
        public NskuForFixedPriceSpecification(Product product, List<Promotion> promotions, IDictionary<string, int> basePrices)
        {
            _product = product;
            _promotions = promotions;
            _basePrices = basePrices;
        }

        public override Expression<Func<Product, Product>> ToExpression()
        {
            var promotion = _promotions.Where(p => p.SkuId == _product.Key).FirstOrDefault();
            double actualPrice = _basePrices[_product.Key] * _product.Quantity;
            _product.Price = actualPrice - (promotion.DiscountPrice * _product.Quantity / promotion.Quantity);
            switch (promotion.Quantity)
            {
                case 1: _product.Price = actualPrice; break;
                default: break;
            }
            return _product => _product;
        }
    }
}
