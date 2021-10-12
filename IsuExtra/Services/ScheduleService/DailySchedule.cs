using System;
using System.Collections.Generic;
using System.Linq;
using IsuExtra.Services.Tools;

namespace IsuExtra.Services.ScheduleService
{
    public class DailySchedule
    {
        private readonly List<Classes> _classes;
        public DailySchedule(DayOfWeek day)
        {
            DayOfWeek = day;
            _classes = new List<Classes>();
        }

        public DayOfWeek DayOfWeek { get; }

        public void AddClasses(Classes classes)
        {
            CheckClassesAreNotIntersected(classes.StartTime, classes.EndTime);
            _classes.Add(classes);
        }

        private static int GetTimeInMinutes(Time time)
        {
            const int minutesInHour = 60;
            return (time.Hour * minutesInHour) + time.Minute;
        }

        private bool AreTimesIntersected(int startTime1, int endTime1, int startTime2, int endTime2)
        {
            static void Swap(ref int a, ref int b)
            {
                (a, b) = (b, a);
            }

            if (startTime1 > startTime2)
            {
                Swap(ref startTime1, ref startTime2);
                Swap(ref endTime1, ref endTime2);
            }

            if (endTime2 <= endTime1) return true;

            return startTime2 <= endTime1 && endTime1 <= endTime2;
        }

        private void CheckClassesAreNotIntersected(Time startTime, Time endTime)
        {
            int startTimeInMinute = GetTimeInMinutes(startTime);
            int endTimeInMinute = GetTimeInMinutes(endTime);

            Classes overLappingClasses = _classes
                .FirstOrDefault(p => AreTimesIntersected(startTimeInMinute, endTimeInMinute, GetTimeInMinutes(p.StartTime), GetTimeInMinutes(p.EndTime)));

            if (overLappingClasses != null)
            {
                throw new IsuExtraException(
                    $"Classes intersected: First class: {startTime.Hour} : {startTime.Minute}, " +
                    $"Second class {overLappingClasses.StartTime.Hour} : {overLappingClasses.StartTime.Minute}");
            }
        }
    }
}