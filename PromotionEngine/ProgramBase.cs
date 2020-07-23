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
                new Promotion{IsActive=true,SkuId="A",Quantity=3,DiscountPrice=20},
                new Promotion{IsActive=true,SkuId="B",Quantity=2,DiscountPrice=15},
                new Promotion{IsActive=true,SkuId="C", Quantity=3},
                new Promotion{IsActive=true,SkuId="D",Quantity=1}
            };
            basePrices = new Dictionary<string, int>() { { "A", 50 }, { "B", 30 }, { "C", 20 }, { "D", 15 } };
        }

        protected static double BuyNitemsForFixedPrice(IDictionary<string, int> cart)
        {
            double totalPrice = default;
            Product p;
            foreach (var product in cart)
            {
                p = new Product() { Key = product.Key, Quantity = product.Value };
                var nItemsForFixedPriceSpecification = new NitemsForFixedPriceSpecification(p, _promotions, basePrices);
                totalPrice += nItemsForFixedPriceSpecification.IsSatisfiedBy(p);
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
        NitemsFixedPrice
    }
}
