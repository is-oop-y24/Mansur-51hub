using System;

namespace IsuExtra.Services.ScheduleService
{
    public class Room
    {
        public Room(string number, Address address)
        {
            Number = number ?? throw new ArgumentNullException(nameof(number));
            Address = address;
        }

        public string Number { get; }
        public Address Address { get; }
    }
}