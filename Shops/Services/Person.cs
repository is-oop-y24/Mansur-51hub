namespace Shops.Services
{
    public class Person
    {
        public Person(string name, double cash)
        {
            Name = name;
            Cash = cash;
        }

        public string Name { get; }
        public double Cash { get; set; }
    }
}