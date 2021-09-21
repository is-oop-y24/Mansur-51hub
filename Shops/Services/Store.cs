using System.Collections.Generic;
using System.Linq;
using Shops.Tools;

namespace Shops.Services
{
    public class Store
    {
        public Store(int id, string name, Address address)
        {
            Id = id;
            Name = name;
            Address = address;
            Products = new List<Product>();
        }

        public int Id { get; }
        public string Name { get; }
        public Address Address { get; }
        public List<Product> Products { get; }

        public Product FindProduct(int productId)
        {
            return Products.SingleOrDefault(p => p.Id.Equals(productId));
        }

        public void AddProduct(Product product)
        {
            if (FindProduct(product.Id) == null)
            {
                Products.Add(product);
                return;
            }

            Product existingProduct = FindProduct(product.Id);
            existingProduct.Count += product.Count;
            existingProduct.Price = product.Price;
        }

        public void ChangeProductPrice(int productId, double newPrice)
        {
            if (FindProduct(productId) == null)
            {
                throw new ShopsException($"No such product with id: {productId} in store '{Name}'");
            }

            FindProduct(productId).Price = newPrice;
        }

        public double GetProductPrice(int productId)
        {
            if (FindProduct(productId) == null)
            {
                throw new ShopsException($"There is no product with id: {productId} in shop with name: {Name} and id {Id}");
            }

            return FindProduct(productId).Price;
        }

        public void BuyProducts(List<Product> products, Person person)
        {
            if (!ContainsProducts(products))
            {
                throw new ShopsException($"Store with id {Id} does not contain all products");
            }

            bool successfulShopping = TryCalculateTotalAmount(products, out double totalAmount);

            if (!successfulShopping)
            {
                throw new ShopsException($"There are not enough products in store with id: {Id}");
            }

            if (person.Cash.CompareTo(totalAmount) < 0)
            {
                throw new ShopsException(
                    $"Not enough money for shopping: totalAmount = {totalAmount}, your money = {person.Cash}");
            }

            foreach (Product product in products)
            {
                FindProduct(product.Id).Count -= product.Count;
            }

            person.Cash -= totalAmount;
        }

        public bool ContainsProducts(List<Product> products)
        {
            foreach (Product product in products)
            {
                if (FindProduct(product.Id) == null)
                {
                    return false;
                }

                if (FindProduct(product.Id).Count < product.Count)
                {
                    return false;
                }
            }

            return true;
        }

        public bool TryCalculateTotalAmount(List<Product> products, out double totalAmount)
        {
            if (!ContainsProducts(products))
            {
                totalAmount = 0;
                return false;
            }

            totalAmount = products.Sum(product => FindProduct(product.Id).Price * product.Count);
            return true;
        }
    }
}