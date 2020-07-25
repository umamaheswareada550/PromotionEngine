using PromotionEngine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
                new Promotion{IsActive=true,PromotionType=PromotionType.NskuFixedPrice,SkuId="C",Quantity=1},
                new Promotion{IsActive=true,PromotionType=PromotionType.NskuFixedPrice,SkuId="D",Quantity=1}
            };
            basePrices = new Dictionary<string, int>() { { "A", 50 }, { "B", 30 }, { "C", 20 }, { "D", 15 } };
        }

        protected static List<Product> GetProductsWithPrice(IDictionary<string, int> cart)
        {
            List<Product> products = new List<Product>();

            //n items Fixed Price Prmotion
            var promotions = _promotions.Where(p => p.PromotionType == PromotionType.NitemsFixedPrice).ToList();
            List<KeyValuePair<string, int>> list = cart.Where(c => promotions.Any(a => a.SkuId == c.Key)).ToList();
            products.AddRange(GetProductsWithPrice<NitemsForFixedPriceSpecification>(list, promotions));

            //n sku's Fixed Price Prmotion
            promotions = _promotions.Where(p => p.PromotionType == PromotionType.NskuFixedPrice).ToList();
            list = cart.Where(c => promotions.Any(a => a.SkuId == c.Key)).ToList();
            products.AddRange(GetProductsWithPrice<NskuForFixedPriceSpecification>(list, promotions));

            return products;
        }

        private static List<Product> GetProductsWithPrice<TSpec>(List<KeyValuePair<string, int>> list, List<Promotion> promotions) where TSpec : new()
        {
            var products = new List<Product>();
            foreach (var p in list)
            {
                Product product = new Product() { Key = p.Key, Quantity = p.Value };
                Specification<Product> spec = Activator.CreateInstance(typeof(TSpec), product, promotions, basePrices) as Specification<Product>;
                var pro = spec.IsSatisfiedBy(product);
                products.Add(pro);
            }
            return products;
        }

        protected static List<Product> BuyNskusForFixedPrice(IDictionary<string, int> cart)
        {
            Product product;
            List<Product> products = new List<Product>();
            var nItemsFixedPricePromotions = _promotions.Where(p => p.PromotionType == PromotionType.NitemsFixedPrice).ToList();
            var list = cart.Where(s => nItemsFixedPricePromotions.Any(a => a.SkuId == s.Key)).ToList();
            foreach (var p in list)
            {
                product = new Product() { Key = p.Key, Quantity = p.Value };
                var nItemsForFixedPriceSpecification = new NitemsForFixedPriceSpecification(new Product() { Key = p.Key, Quantity = p.Value }, nItemsFixedPricePromotions, basePrices);
                products.Add(nItemsForFixedPriceSpecification.IsSatisfiedBy(product));
            }
            return products;
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
