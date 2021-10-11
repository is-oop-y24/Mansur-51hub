using System.Collections.Generic;
using System.Linq;
using Isu.Services;
using IsuExtra.Services.Tools;

namespace IsuExtra.Services.ScheduleService
{
    public class BasicScheduleService : IScheduleService
    {
        private readonly List<WeeklyScheduleForGroup> _groupSchedules;

        public BasicScheduleService()
        {
            _groupSchedules = new List<WeeklyScheduleForGroup>();
        }

        public IReadOnlyList<WeeklyScheduleForGroup> GetAllGroupsSchedule()
        {
            return _groupSchedules;
        }

        public WeeklyScheduleForGroup GetGroupSchedule(IsuGroupName name)
        {
            WeeklyScheduleForGroup existingScheduleForGroup = FindGroupSchedule(name);
            if (existingScheduleForGroup != null)
            {
                return existingScheduleForGroup;
            }

            throw new IsuExtraException($"Can not find group with name {name.Name} in schedule");
        }

        public void SetGroupSchedule(IsuGroupName name, WeeklySchedule newWeeklySchedule)
        {
            WeeklyScheduleForGroup existingScheduleForGroup = FindGroupSchedule(name);
            if (existingScheduleForGroup == null)
            {
                _groupSchedules.Add(new WeeklyScheduleForGroup(name, newWeeklySchedule));
                return;
            }

            existingScheduleForGroup.WeeklySchedule.ChangeWeeklySchedule(newWeeklySchedule.GetWeeklySchedule().ToList());
        }

        private WeeklyScheduleForGroup FindGroupSchedule(IsuGroupName groupName)
        {
            return _groupSchedules.FirstOrDefault(p => p.GroupName.Name.Equals(groupName.Name));
        }
    }
}