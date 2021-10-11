using System;

namespace IsuExtra.Services.ScheduleService
{
    public class Address
    {
        public Address(string streetName, string houseNumber)
        {
            StreetName = streetName ?? throw new ArgumentNullException(nameof(streetName));
            HouseNumber = houseNumber ?? throw new ArgumentNullException(nameof(houseNumber));
        }

        public string StreetName { get; }
        public string HouseNumber { get; }
    }
}