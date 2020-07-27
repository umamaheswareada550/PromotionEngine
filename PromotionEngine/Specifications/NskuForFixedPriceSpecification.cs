//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;

//namespace PromotionEngine.Specifications
//{
//    public class NskuForFixedPriceSpecification : Specification<Product>
//    {
//        private List<Product> _list;
//        private IDictionary<string, int> _basePrices;
//        private Promotion _promotion;


//        public NskuForFixedPriceSpecification()
//        {
//        }
//        public NskuForFixedPriceSpecification(List<Product> list, Promotion prom, IDictionary<string, int> basePrices)
//        {
//            _basePrices = basePrices;
//            _list = list;
//        }

//        public override Expression<Func<Product, Product>> ToExpression()
//        {
//            _list.LastOrDefault().Price = _promotion.DiscountPrice;
//            //var promotion = _promotions.Where(p => p.SkuId == _product.Key).FirstOrDefault();
//            //double actualPrice = _basePrices[_product.Key] * _product.Quantity;
//            //_product.Price = actualPrice - (promotion.DiscountPrice * _product.Quantity / promotion.Quantity);
//            //switch (promotion.Quantity)
//            //{
//            //    case 1: _product.Price = actualPrice; break;
//            //    default: break;
//            //}
//            return _product => _product;
//        }
//    }
//}
