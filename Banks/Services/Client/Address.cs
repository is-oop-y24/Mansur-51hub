using System;

namespace Banks.Services.Client
{
    public class Address
    {
        public Address(string cityName, string streetName, string houseNumber)
        {
            CityName = cityName ?? throw new ArgumentNullException(nameof(cityName));
            StreetName = streetName ?? throw new ArgumentNullException(nameof(streetName));
            HouseNumber = houseNumber ?? throw new ArgumentNullException(nameof(houseNumber));
        }

        public string CityName { get; }
        public string StreetName { get; }
        public string HouseNumber { get; }
    }
}