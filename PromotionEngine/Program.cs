using System;
using System.Collections.Generic;
using System.Linq;

namespace PromotionEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            IDictionary<string, double> dict = new Dictionary<string, double>()
            {
                { "A", 50 },
                { "B", 30 },
                { "C", 20 },
                { "D", 15 }
            };

            IDictionary<string, double> cart = new Dictionary<string, double>() { };
            foreach (var item in dict)
            {
                Console.WriteLine("Enter the quantity for " + item.Key);
                var quantity = Convert.ToInt32(Console.ReadLine());
                if (quantity != 0) //ignore 0 quantities
                {
                    cart.Add(new KeyValuePair<string, double>(item.Key, item.Value * quantity));//price
                }
            }

            Console.WriteLine("Cart Summary :" + string.Join(',', cart.Select(c => c.Key + "(" + c.Value + ")")));
            Console.WriteLine("Total Price: " + cart.Sum(c => c.Value));
            Console.ReadLine();
        }
    }
}
