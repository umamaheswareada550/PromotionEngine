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

        public NitemsForFixedPriceSpecification()
        {
        }
        public NitemsForFixedPriceSpecification(Product product, List<Promotion> promotions, IDictionary<string, int> basePrices)
        {
            _product = product;
            _promotions = promotions;
            _basePrices = basePrices;
        }

        public override Expression<Func<Product, Product>> ToExpression()
        {
            var promotion = _promotions.Where(p => p.SkuId == _product.Key).FirstOrDefault();
            double actualPrice = _basePrices[_product.Key] * _product.Quantity;
            if (_product.Quantity % promotion.Quantity == 0)
                _product.Price = actualPrice - (promotion.DiscountPrice * _product.Quantity / promotion.Quantity);
            else
                _product.Price = actualPrice;

            return _product => _product;
        }
    }
}
