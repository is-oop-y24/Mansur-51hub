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
        private readonly List<RegisteredProductInService> _products;

        public BasicMarketService()
        {
            _stores = new List<Store>();
            _storeIdGenerator = new IdGenerator();
            _productIdGenerator = new IdGenerator();
            _products = new List<RegisteredProductInService>();
        }

        public RegisteredProductInService RegisterProduct(string name)
        {
            if (FindRegisteredProductInService(name) != null)
            {
                throw new ShopsException($"Product with name {name} has already registered in service");
            }

            var newProduct = new RegisteredProductInService(_productIdGenerator.GenerateId(), name);
            _products.Add(newProduct);
            return newProduct;
        }

        public void DeliverProductsToStore(List<Product> products, int storeId)
        {
            Store requiredStore = FindStore(storeId);
            if (requiredStore == null)
            {
                throw new ShopsException($"There is no store with store id {storeId} in market service");
            }

            foreach (Product product in products)
            {
                if (FindRegisteredProductInService(product.Id) == null)
                {
                    throw new ShopsException($"There is no registered product with id {product.Id} in market service");
                }

                requiredStore.AddProduct(product);
            }
        }

        public void BuyProducts(List<Product> products, int storeId, Person person)
        {
            Store requiredStore = FindStore(storeId);
            if (requiredStore == null)
            {
                throw new ShopsException($"There is no store with id {storeId} in market service");
            }

            CheckAreProductsRegistered(products);

            requiredStore.BuyProducts(products, person);
        }

        public Store FindTheCheapestStore(List<Product> products)
        {
            var suitableStores =
                _stores.Where(p => p.TryCalculateTotalAmount(products, out double amount) == true).ToList();
            if (suitableStores.Count == 0)
            {
                return null;
            }

            return suitableStores.Aggregate((store1, store2) =>
                store1.GetTotalAmount(products) < store2.GetTotalAmount(products) ? store1 : store2);
        }

        public IReadOnlyList<Product> GetProducts(int storeId)
        {
            Store requiredStore = FindStore(storeId);
            if (requiredStore == null)
            {
                throw new ShopsException($"There is no store with id {storeId} in market service");
            }

            return requiredStore.GetProducts();
        }

        public IReadOnlyList<Store> GetStores()
        {
            return _stores;
        }

        public List<RegisteredProductInService> GetRegisteredProducts()
        {
            return _products;
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
            return _products.FirstOrDefault(p => p.Name.Equals(name));
        }

        public RegisteredProductInService FindRegisteredProductInService(int productId)
        {
            return _products.FirstOrDefault(p => p.Id.Equals(productId));
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