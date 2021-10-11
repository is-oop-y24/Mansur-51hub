using System.Collections.Generic;
using Isu.Services;

namespace IsuExtra.Services.ScheduleService
{
    public interface IScheduleService
    {
        WeeklyScheduleForGroup GetGroupSchedule(IsuGroupName name);
        void SetGroupSchedule(IsuGroupName name, WeeklySchedule newWeeklySchedule);
        IReadOnlyList<WeeklyScheduleForGroup> GetAllGroupsSchedule();
    }
}