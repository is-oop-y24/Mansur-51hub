using System.Collections.Generic;
using System.Linq;
using IsuExtra.Services.Tools;

namespace IsuExtra.Services.ScheduleService
{
    public class WeeklySchedule
    {
        private List<DailySchedule> _weeklySchedule;

        public WeeklySchedule(List<DailySchedule> weeklySchedule)
        {
            CheckTheCorrectionOfTheSchedule(weeklySchedule);
            _weeklySchedule = weeklySchedule;
        }

        public void ChangeWeeklySchedule(List<DailySchedule> weeklySchedule)
        {
            CheckTheCorrectionOfTheSchedule(weeklySchedule);
            _weeklySchedule = weeklySchedule;
        }

        public IReadOnlyList<DailySchedule> GetWeeklySchedule()
        {
            return _weeklySchedule;
        }

        public void CheckClassesAreNotIntersected(Classes classes, DayOfWeek dayOfWeek)
        {
            DailySchedule requiredSchedule = _weeklySchedule.FirstOrDefault(p => p.DayOfWeek.Equals(dayOfWeek));
            if (requiredSchedule == null)
            {
                return;
            }

            DailySchedule checkedSchedule = requiredSchedule;
            checkedSchedule.AddClasses(classes);
        }

        private void CheckTheCorrectionOfTheSchedule(List<DailySchedule> weeklySchedule)
        {
            var days = new List<int>()
            {
                (int)DayOfWeek.Monday,
                (int)DayOfWeek.Tuesday,
                (int)DayOfWeek.Wednesday,
                (int)DayOfWeek.Thursday,
                (int)DayOfWeek.Friday,
                (int)DayOfWeek.Saturday,
                (int)DayOfWeek.Sunday,
            };

            if (days
                .Any(day => weeklySchedule
                    .Count(weekDay => ((int)weekDay.DayOfWeek).Equals(day)) > 1))
            {
                throw new IsuExtraException($"Error weekly schedule format: some day repeats more then once");
            }
        }
    }
}