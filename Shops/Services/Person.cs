using System;

namespace Shops.Services
{
    public class Person
    {
        public Person(string name, double cash)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Cash = cash;
        }

        public string Name { get; }
        public double Cash { get; set; }
    }
}