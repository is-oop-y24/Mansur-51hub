using System.Collections.Generic;
using System.Linq;
using Shops.Tools;

namespace Shops.Services
{
    public class BasicMarketService : IMarketService
    {
        private readonly List<Store> _stores;
        private readonly IdGenerator _storeIdGenerator;
        private readonly IdGenerator _productIdGenerator;

        public BasicMarketService()
        {
            _stores = new List<Store>();
            _storeIdGenerator = new IdGenerator();
            _productIdGenerator = new IdGenerator();
            Products = new List<RegisteredProductInService>();
        }

        public List<RegisteredProductInService> Products { get; }

        public RegisteredProductInService RegisterProduct(string name)
        {
            if (FindRegisteredProductInService(name) != null)
            {
                throw new ShopsException($"Product with name {name} has already registered in service");
            }

            var newProduct = new RegisteredProductInService(_productIdGenerator.GenerateId(), name);
            Products.Add(newProduct);
            return newProduct;
        }

        public void DeliverProductsToStore(List<Product> products, int storeId)
        {
            if (FindStore(storeId) == null)
            {
                throw new ShopsException($"There is no store with store id {storeId} in market service");
            }

            foreach (Product product in products)
            {
                if (FindRegisteredProductInService(product.Id) == null)
                {
                    throw new ShopsException($"There is no registered product with id {product.Id} in market service");
                }

                FindStore(storeId).AddProduct(product);
            }
        }

        public void BuyProducts(List<Product> products, int storeId, Person person)
        {
            if (FindStore(storeId) == null)
            {
                throw new ShopsException($"There is no store with id {storeId} in market service");
            }

            CheckAreProductsRegistered(products);

            FindStore(storeId).BuyProducts(products, person);
        }

        public Store FindTheCheapestStore(List<Product> products)
        {
            double minimalAmount = double.MaxValue;
            Store cheapestStore = null;

            foreach (Store store in _stores)
            {
                if (!store.TryCalculateTotalAmount(products, out double currentAmount)) continue;
                if (currentAmount.CompareTo(minimalAmount) >= decimal.Zero) continue;
                minimalAmount = currentAmount;
                cheapestStore = store;
            }

            return cheapestStore;
        }

        public List<Product> GetProducts(int storeId)
        {
            if (FindStore(storeId) == null)
            {
                throw new ShopsException($"There is no store with id {storeId} in market service");
            }

            return FindStore(storeId).Products;
        }

        public List<Store> GetStores()
        {
            return _stores;
        }

        public List<RegisteredProductInService> GetRegisteredProducts()
        {
            return Products;
        }

        public Store AddStore(string name, Address address)
        {
            if (FindStore(name, address) != null)
            {
                throw new ShopsException(
                    $"Shop with name {name} and address: {address.CityName}, {address.StreetName}, {address.HouseNumber} already exists");
            }

            var store = new Store(_storeIdGenerator.GenerateId(), name, address);
            _stores.Add(store);
            return store;
        }

        public Store FindStore(string name, Address address)
        {
            return _stores.FirstOrDefault(s => s.Name.Equals(name) &&
                                               $"{s.Address.CityName}{s.Address.HouseNumber}{s.Address.StreetName}"
                                                   .Equals($"{address.CityName}{address.HouseNumber}{address.StreetName}"));
        }

        public Store FindStore(int storeId)
        {
            return _stores.FirstOrDefault(s => s.Id.Equals(storeId));
        }

        public Store GetStore(int storeId)
        {
            return _stores.First(s => s.Id.Equals(storeId));
        }

        public RegisteredProductInService FindRegisteredProductInService(string name)
        {
            return Products.FirstOrDefault(p => p.Name.Equals(name));
        }

        public RegisteredProductInService FindRegisteredProductInService(int productId)
        {
            return Products.FirstOrDefault(p => p.Id.Equals(productId));
        }

        private void CheckAreProductsRegistered(List<Product> products)
        {
            Product product = products.FirstOrDefault(p => FindRegisteredProductInService(p.Id) == null);
            if (product != null)
            {
                throw new ShopsException(
                    $"There is no registered product with id: {product.Id} and name: {product.Name}");
            }
        }
    }
}