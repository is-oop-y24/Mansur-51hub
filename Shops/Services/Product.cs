using System;
using Shops.Tools;

namespace Shops.Services
{
    public class Product
    {
        public Product(int id, string name, double price, int count)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            if (count < 0)
            {
                throw new ShopsException("Count can not be o negative number");
            }

            Count = count;
            if (Price.CompareTo(0) < 0)
            {
                throw new ShopsException("Price could not be a negative number");
            }

            Price = price;
        }

        public int Id { get; }
        public string Name { get; }
        public int Count { get; set; }
        public double Price { get; set; }
    }
}