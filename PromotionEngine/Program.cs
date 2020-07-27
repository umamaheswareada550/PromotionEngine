using System;
using System.Collections.Generic;
using System.Linq;

namespace PromotionEngine
{
    public class Program : ProgramBase
    {
        static void Main(string[] args)
        {
            IDictionary<string, int> cart = new Dictionary<string, int>() { };//{key,price}
            foreach (var item in basePrices)
            {
                Console.WriteLine("Enter the quantity for " + item.Key);
                var quantity = Convert.ToInt32(Console.ReadLine());
                if (quantity > 0) //ignore lessthan or equal to 0 quantities
                {
                    cart.Add(new KeyValuePair<string, int>(item.Key, quantity));
                }
            }
            Console.WriteLine("Cart Summary :" + string.Join(',', cart.Select(c => c.Key + "(" + c.Value + ")")));

            List<Product> products = GetItemsPrices(cart);

            Console.WriteLine(string.Format("Total Price: {0} = {1}", string.Join('+', products.Select(c => c.Price)), products.Sum(c => c.Price)));
            Console.ReadLine();
        }
    }
}
