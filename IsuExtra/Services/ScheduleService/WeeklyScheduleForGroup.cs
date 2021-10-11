using Isu.Services;

namespace IsuExtra.Services.ScheduleService
{
    public class WeeklyScheduleForGroup
    {
        public WeeklyScheduleForGroup(IsuGroupName groupName, WeeklySchedule weeklySchedule)
        {
            GroupName = groupName;
            WeeklySchedule = weeklySchedule;
        }

        public IsuGroupName GroupName { get; }
        public WeeklySchedule WeeklySchedule { get; }
    }
}