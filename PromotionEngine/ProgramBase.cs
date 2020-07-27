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
                new Promotion{IsActive=true,SkuId="A",Quantity=3,DiscountPrice=20},
                new Promotion{IsActive=true,SkuId="B",Quantity=2,DiscountPrice=15},
                new Promotion{IsActive=true,SkuId="C",Quantity=1,DiscountPrice=20},
                new Promotion{IsActive=true,SkuId="D",Quantity=1,DiscountPrice=50},
                new Promotion{IsActive=true,SkuId="C,D",Quantity=1,DiscountPrice=30}
            };
            basePrices = new Dictionary<string, int>() { { "A", 50 }, { "B", 30 }, { "C", 20 }, { "D", 15 } };
        }

        //n items Fixed Price Prmotion
        protected static List<Product> GetNitemsForFixedPrice(IDictionary<string, int> cart)
        {
            List<Product> products = new List<Product>();
            object[] parameters;
            //var promotions = _promotions.Where(p => p.PromotionType == PromotionType.NitemsFixedPrice).ToList();
            Promotion promotion = default;
            var list = cart.Where(s => _promotions.Any(p => p.SkuId == s.Key)).ToList();
            foreach (var p in list)
            {
                Product product = new Product() { Key = p.Key, Quantity = p.Value };
                promotion = _promotions.Where(prom => prom.SkuId == p.Key).FirstOrDefault();
                parameters = new object[] { product, promotion, basePrices };
                products.Add(GetProductsWithPrice<NitemsForFixedPriceSpecification, Product>(product, parameters));
            }

            return products;
        }

        //n sku's Fixed Price Promotion
        protected static List<Product> GetItemsPrices(IDictionary<string, int> cart)
        {
            List<Product> products = new List<Product>();
            var combinationPromotions = _promotions.Where(p => p.SkuId.Split(',').Count() > 1).ToList();
            foreach (var prom in combinationPromotions)
            {
                bool hasAllPromotionKeysinCart = cart.Keys.ToArray().Intersect(prom.SkuId.Split(',').ToArray()).Count() > 1; //.All(p=> combinationPromotions.Contains(p.Key) 
                if (hasAllPromotionKeysinCart)
                {
                    var combinationKeys = prom.SkuId.Split(',');
                    foreach (var key in combinationKeys)
                    {
                        products.AddRange(cart.Where(s => key == s.Key).Select(s => new Product() { Key = s.Key, Quantity = s.Value }).ToList());

                        //Since already processed the combination product, remove it in the cart
                        cart.Remove(key);

                    }
                    products.LastOrDefault().Price = combinationPromotions.FirstOrDefault().DiscountPrice;
                }
                else
                {
                    var nonPromotionKeys = prom.SkuId.Split(',').Intersect(cart.Keys.ToArray());
                    foreach (var key in nonPromotionKeys)
                    {
                        products.AddRange(cart.Where(s => key == s.Key).Select(s => new Product() { Key = s.Key, Quantity = s.Value, Price = basePrices[s.Key] * s.Value }).ToList());
                        
                        //Since already processed the combination product, remove it in the cart
                        cart.Remove(key);
                    }
                }
            }

            if (combinationPromotions.Count == 0 || cart.Count > 0)
            {
                products.AddRange(GetNitemsForFixedPrice(cart));
            }

            return products;
        }

        private static TEntity GetProductsWithPrice<TSpec, TEntity>(TEntity entity, params object[] parameters) where TSpec : new()
        {
            Specification<TEntity> spec = Activator.CreateInstance(typeof(TSpec), parameters) as Specification<TEntity>;
            return spec.IsSatisfiedBy(entity);
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
