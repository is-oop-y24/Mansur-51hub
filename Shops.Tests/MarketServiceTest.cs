using System.Collections.Generic;
using NUnit.Framework;
using Shops.Services;
using Shops.Tools;

namespace Shops.Tests
{
    public class MarketServiceTest
    {
        private IMarketService _marketService;

        [SetUp]
        public void SetUp()
        {
            _marketService = new BasicMarketService();
        }

        [Test]
        public void RegisterProductInService_ServiceContainsProduct()
        {
            _marketService.RegisterProduct("Milka");
            Assert.AreNotEqual(_marketService.FindRegisteredProductInService("Milka"), null);
        }

        [Test]
        public void ChangeProductPriceInStore_PriceHasChanged()
        {
            _marketService.AddStore("ItmoShop", new Address("Spb", "Kronverskaya", "19"));
            _marketService.RegisterProduct("Milka");
            int storeId = 1;
            int productId = 1;
            double productPrice = 24;
            int ProductCount = 5;
            double newProductPrice = 50;
            Store store = _marketService.GetStore(storeId);
            store.AddProduct(new Product(productId, "Milka", productPrice, ProductCount));
            store.ChangeProductPrice(productId, newProductPrice);
            Assert.AreNotEqual(store.GetProductPrice(productId), productPrice);
        }

        [Test]
        public void RegisterProductsInServiceAndDeliverProductsToStore_StoreContainsProductsAndWeCanBuyProducts_ProductsCountChangedAndPersonCashValueChanged()
        {
            string productName1 = "Milka";
            string productName2 = "Alenka";
            string productName3 = "Babaevskiy";
            _marketService.RegisterProduct(productName1);
            _marketService.RegisterProduct(productName2);
            _marketService.RegisterProduct(productName3);

            int productId1 = 1;
            int productId2 = 2;
            int productId3 = 3;

            double price1 = 22;
            double price2 = 23;
            double price3 = 31;

            int productCount1 = 12;
            int productCount2 = 34;
            int productCount3 = 18;

            var products = new List<Product>
            {
                new Product(productId1, productName1, price1, productCount1),
                new Product(productId2, productName2, price2, productCount2),
                new Product(productId3, productName3, price3, productCount3)
            };

            _marketService.AddStore("Lenta", new Address("Spb", "Lenina", "23b"));
            int storeId = 1;
            _marketService.DeliverProductsToStore(products, storeId);

            Store store = _marketService.GetStore(storeId);
            Assert.AreEqual(true, store.ContainsProducts(products));

            double cash = 15000;
            var person = new Person("Itmo first course student", cash);
            store.BuyProducts(products, person);
            double newCash = person.Cash;
            Assert.AreNotEqual(cash, newCash);
            Assert.AreNotEqual(store.FindProduct(productId1).Count, productCount1);
            Assert.AreNotEqual(store.FindProduct(productId2).Count, productCount2);
            Assert.AreNotEqual(store.FindProduct(productId3).Count, productCount3);
        }

        [Test]
        public void FindTheCheapestStore_NotEnoughMoneyAndStoreNotNull_ThrowException()
        {
            Assert.Catch<ShopsException>(() =>
            {
                string productName1 = "Milka";
                string productName2 = "Alenka";
                _marketService.RegisterProduct(productName1);
                _marketService.RegisterProduct(productName2);

                int productId1 = 1;
                int productId2 = 2;

                double price1 = 22;
                double price2 = 23;

                int productCount1 = 12;
                int productCount2 = 34;


                var products = new List<Product>
                {
                    new Product(productId1, productName1, price1, productCount1),
                    new Product(productId2, productName2, price2, productCount2),
                };

                _marketService.AddStore("Lenta", new Address("Spb", "Lenina", "23b"));
                int storeId = 1;
                _marketService.DeliverProductsToStore(products, storeId);

                var person = new Person("Itmo second year student", 200);
                Store store = _marketService.FindTheCheapestStore(products);
                Assert.AreNotEqual(null, store);
                store.BuyProducts(products, person);
            });
        }

        [Test]
        public void BuyUnregisteredProduct_ThrowException()
        {
            Assert.Catch<ShopsException>(() =>
            {
                var product = new Product(3, "365days", 21, 13);
                _marketService.AddStore("StoreName", new Address("City", "Street", "HouseNumber"));
                int storeId = 1;
                Store store = _marketService.GetStore(storeId);
                double cash = 100;
                store.BuyProducts(new List<Product>{product}, new Person("Person", cash));
            });
        }

        [Test]
        public void FindTheCheapestStore_NotEnoughProducts_ReturnsNull()
        {
            string productName1 = "Milka";
            string productName2 = "Alenka";
            _marketService.RegisterProduct(productName1);
            _marketService.RegisterProduct(productName2);

            int productId1 = 1;
            int productId2 = 2;

            double price1 = 22;
            double price2 = 23;

            int productCount1 = 12;
            int productCount2 = 34;


            var products = new List<Product>
            {
                new Product(productId1, productName1, price1, productCount1),
                new Product(productId2, productName2, price2, productCount2),
            };

            _marketService.AddStore("Lenta", new Address("Spb", "Lenina", "23b"));
            int storeId = 1;

            var deliveredProducts = new List<Product>
            {
                new Product(productId1, productName1, price1, productCount1 - 1),
                new Product(productId2, productName2, price2, productCount2 - 1),
            };

            _marketService.DeliverProductsToStore(deliveredProducts, storeId);

            Store store = _marketService.FindTheCheapestStore(products);
            Assert.AreEqual(null, store);
        }
    }
}