using System;

namespace IsuExtra.Services.ScheduleService
{
    public class Time
    {
        public Time(int hour, int minute)
        {
            const int minHour = 0;
            const int maxHour = 23;
            const int minMinute = 0;
            const int maxMinute = 59;

            if (hour is < minHour or > maxHour)
            {
                throw new ArgumentOutOfRangeException(nameof(hour));
            }

            if (minute is < minMinute or > maxMinute)
            {
                throw new ArgumentOutOfRangeException(nameof(minute));
            }

            Hour = hour;
            Minute = minute;
        }

        public int Hour { get; }
        public int Minute { get; }
    }
}