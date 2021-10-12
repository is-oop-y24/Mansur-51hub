using System;

namespace IsuExtra.Services.ScheduleService
{
    public class Tutor
    {
        public Tutor(string name, int id)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Id = id;
        }

        public string Name { get; }
        public int Id { get; }
    }
}