using PromotionEngine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PromotionEngine
{
    public class ProgramBase
    {
        protected static List<Promotion> _promotions;
        protected static IDictionary<string, int> basePrices;

        static ProgramBase()
        {
            _promotions = new List<Promotion>
            {
                new Promotion{IsActive=true,PromotionType=PromotionType.NitemsFixedPrice, SkuId="A",Quantity=3,DiscountPrice=20},
                new Promotion{IsActive=true,PromotionType=PromotionType.NitemsFixedPrice,SkuId="B",Quantity=2,DiscountPrice=15},
                new Promotion{IsActive=true,PromotionType=PromotionType.NskuFixedPrice,SkuId="C", Quantity=3},
                new Promotion{IsActive=true,PromotionType=PromotionType.NskuFixedPrice,SkuId="D",Quantity=1}
            };
            basePrices = new Dictionary<string, int>() { { "A", 50 }, { "B", 30 }, { "C", 20 }, { "D", 15 } };
        }

        protected static double BuyNitemsForFixedPrice(IDictionary<string, int> cart)
        {
            List<Product> products = new List<Product>();
            double totalPrice = default;
            Product product = default;

            //n items Fixed Price Prmotion
            var promotions = _promotions.Where(p => p.PromotionType == PromotionType.NitemsFixedPrice).ToList();
            var list = cart.Where(s => promotions.Any(a => a.SkuId == s.Key)).ToList();
            foreach (var p in list)
            {
                product = new Product() { Key = p.Key, Quantity = p.Value };
                var nItemsForFixedPriceSpecification = new NitemsForFixedPriceSpecification(product, promotions, basePrices);
                totalPrice += nItemsForFixedPriceSpecification.IsSatisfiedBy(product);
            }

            return totalPrice;
        }
    }

    public class Product
    {
        public string Key { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }

    public class Promotion
    {
        public PromotionType PromotionType { get; set; }
        public bool IsActive { get; set; }
        public int Quantity { get; set; }
        public string SkuId { get; set; }
        public double DiscountPrice { get; set; }
    }

    public enum PromotionType
    {
        NitemsFixedPrice,
        NskuFixedPrice
    }
}
