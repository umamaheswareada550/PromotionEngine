using Microsoft.VisualStudio.TestTools.UnitTesting;
using PromotionEngine;
using System.Collections.Generic;

namespace PromotionEngineTests
{
    [TestClass]
    public class PromotionEngineTests
    {

        [TestMethod]
        public void Get_Each_Product_Price_For_1_Quantity()
        {
            IDictionary<string, int> cart = new Dictionary<string, int>() { };
            cart.Add(new KeyValuePair<string, int>("A", 1));
            cart.Add(new KeyValuePair<string, int>("B", 1));
            cart.Add(new KeyValuePair<string, int>("C", 1));

            var products = ProgramBase.GetItemsPrices(cart);

            Assert.AreEqual(products[0].Key, "C");//Combination Promotions are calculated first
            Assert.AreEqual(products[1].Key, "A");
            Assert.AreEqual(products[2].Key, "B");
            Assert.AreEqual(products[0].Price, 20);
            Assert.AreEqual(products[1].Price, 50);
            Assert.AreEqual(products[2].Price, 30);
        }

        [TestMethod]
        public void Get_Each_Product_Price_For_Multiple_Quantities()
        {
            IDictionary<string, int> cart = new Dictionary<string, int>() { };
            cart.Add(new KeyValuePair<string, int>("A", 5));
            cart.Add(new KeyValuePair<string, int>("B", 5));
            cart.Add(new KeyValuePair<string, int>("C", 1));

            var products = ProgramBase.GetItemsPrices(cart);

            Assert.AreEqual(products[0].Key, "C");//Combination Promotions are calculated first
            Assert.AreEqual(products[1].Key, "A");
            Assert.AreEqual(products[2].Key, "B");
            Assert.AreEqual(products[0].Price, 20);
            Assert.AreEqual(products[1].Price, 230);
            Assert.AreEqual(products[2].Price, 120);
        }

        [TestMethod]
        public void Get_Each_Product_Price_For_active_Promotions_With_Different_Quantities()
        {
            IDictionary<string, int> cart = new Dictionary<string, int>() { };
            cart.Add(new KeyValuePair<string, int>("A", 3));
            cart.Add(new KeyValuePair<string, int>("B", 5));
            cart.Add(new KeyValuePair<string, int>("C", 1));
            cart.Add(new KeyValuePair<string, int>("D", 1));

            var products = ProgramBase.GetItemsPrices(cart);

            Assert.AreEqual(products[0].Key, "C");//Combination Promotions are calculated first
            Assert.AreEqual(products[1].Key, "D");
            Assert.AreEqual(products[2].Key, "A");
            Assert.AreEqual(products[3].Key, "B");
            Assert.AreEqual(products[0].Price, 0);
            Assert.AreEqual(products[1].Price, 30);
            Assert.AreEqual(products[2].Price, 130);
            Assert.AreEqual(products[3].Price, 120);
        }
    }
}
