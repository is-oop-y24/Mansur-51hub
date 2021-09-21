namespace Shops.Services
{
    public class Address
    {
        public Address(string cityName, string streetName, string houseNumber)
        {
            CityName = cityName;
            StreetName = streetName;
            HouseNumber = houseNumber;
        }

        public string CityName { get; }
        public string StreetName { get; }
        public string HouseNumber { get; }
    }
}