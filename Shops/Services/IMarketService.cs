using System.Collections.Generic;

namespace Shops.Services
{
    public interface IMarketService
    {
        Store AddStore(string name, Address address);
        RegisteredProductInService RegisterProduct(string name);

        Store FindStore(string name, Address address);
        Store FindStore(int storeId);
        Store GetStore(int storeId);

        void DeliverProductsToStore(List<Product> products, int storeId);
        void BuyProducts(List<Product> products, int storeId, Person person);
        Store FindTheCheapestStore(List<Product> products);

        List<Product> GetProducts(int storeId);
        List<Store> GetStores();
        List<RegisteredProductInService> GetRegisteredProducts();
        RegisteredProductInService FindRegisteredProductInService(string name);
        RegisteredProductInService FindRegisteredProductInService(int productId);
    }
}