using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PromotionEngine.Specifications
{
    public class NitemsForFixedPriceSpecification : Specification<Product>
    {
        protected Promotion _promotion;
        protected IDictionary<string, int> _basePrices;
        private readonly Product _product;

        public NitemsForFixedPriceSpecification()
        {
        }
        public NitemsForFixedPriceSpecification(Product product, Promotion promotion, IDictionary<string, int> basePrices)
        {
            _product = product;
            _promotion = promotion;
            _basePrices = basePrices;
        }

        public override Expression<Func<Product, Product>> ToExpression()
        {
            double actualPrice = _basePrices[_product.Key] * _product.Quantity;
            if (_product.Quantity % _promotion.Quantity == 0)
                _product.Price = actualPrice - _promotion.DiscountPrice * (_product.Quantity / _promotion.Quantity);
            else
                _product.Price = actualPrice - (_product.Quantity / _promotion.Quantity) * _promotion.DiscountPrice;
            return _product => _product;
        }
    }
}
