using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Shops.Tools;

namespace Shops.Services
{
    public class Store
    {
        private readonly List<Product> _products;
        public Store(int id, string name, Address address)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Address = address;
            _products = new List<Product>();
        }

        public int Id { get; }
        public string Name { get; }
        public Address Address { get; }

        public Product FindProduct(int productId)
        {
            return _products.SingleOrDefault(p => p.Id.Equals(productId));
        }

        public void AddProduct(Product product)
        {
            Product requiredProduct = FindProduct(product.Id);
            if (requiredProduct == null)
            {
                _products.Add(product);
                return;
            }

            requiredProduct.Count += product.Count;
            requiredProduct.Price = product.Price;
        }

        public void ChangeProductPrice(int productId, double newPrice)
        {
            Product requiredProduct = FindProduct(productId);
            if (requiredProduct == null)
            {
                throw new ShopsException($"No such product with id: {productId} in store '{Name}'");
            }

            requiredProduct.Price = newPrice;
        }

        public double GetProductPrice(int productId)
        {
            Product requiredProduct = FindProduct(productId);
            if (requiredProduct == null)
            {
                throw new ShopsException($"There is no product with id: {productId} in shop with name: {Name} and id {Id}");
            }

            return requiredProduct.Price;
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
            return products.FirstOrDefault(p =>
            {
                Product productInStore = FindProduct(p.Id);
                return productInStore == null || productInStore.Count < p.Count;
            }) == null;
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

        public double GetTotalAmount(List<Product> products)
        {
            if (!ContainsProducts(products))
            {
                throw new ShopsException($"There are not enough products in store with id {Id}");
            }

            return products.Sum(product => FindProduct(product.Id).Price * product.Count);
        }

        public List<Product> GetProducts()
        {
            return _products;
        }
    }
}