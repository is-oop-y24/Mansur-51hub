using System;
namespace IsuExtra.Services.ScheduleService
{
    public class Classes
    {
        public Classes(string title, Tutor tutor, Room room, Time startTime, Time endTime)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Tutor = tutor;
            Room = room;
            StartTime = startTime;
            EndTime = endTime;
        }

        public string Title { get; }
        public Tutor Tutor { get; }
        public Room Room { get; }
        public Time StartTime { get; }
        public Time EndTime { get; }
    }
}