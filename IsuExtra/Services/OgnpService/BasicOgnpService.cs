using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Services;
using IsuExtra.Services.ScheduleService;
using IsuExtra.Services.Tools;

namespace IsuExtra.Services.OgnpService
{
    public class BasicOgnpService : IOgnpService
    {
        private const int NumberOfOgnp = 1;
        private readonly List<Group> _groups;
        private readonly List<WeeklyScheduleForGroup> _groupsSchedule;
        private readonly List<Ognp> _ognps;

        public BasicOgnpService(IIsuService isuService, IScheduleService scheduleService)
        {
            _ognps = new List<Ognp>();
            var groups = isuService.GetGroups().ToList();
            CheckGroupsAreNotDuplicated(groups);
            _groups = groups;
            var groupsSchedule = scheduleService.GetAllGroupsSchedule().ToList();
            CheckThatAllGroupsHaveSchedule(groupsSchedule);
            _groupsSchedule = groupsSchedule;
        }

        public void AddNewOgnp(Ognp ognp)
        {
            if (ContainsOgnp(ognp.Institute.Name))
            {
                throw new IsuExtraException($"Ognp from school {ognp.Institute.Name} already exists");
            }

            _ognps.Add(ognp);
        }

        public void EnrollStudentToOgnp(Student student, Ognp ognp, StreamNumber streamNumber)
        {
            if (!ContainsStudent(student.Id))
            {
                throw new IsuExtraException($"Student with id: {student.Id} does not exists");
            }

            if (GetStudentOgnpsCount(student.Id) == NumberOfOgnp)
            {
                throw new IsuExtraException($"Student with id: {student.Id} already enrolled in {NumberOfOgnp} ognps");
            }

            if (ognp.Institute.Name.Equals(ItmoInstitutes.GetInstituteFromPrefix(student.GroupName).Name))
            {
                throw new IsuExtraException($"Student can not enroll in his own school");
            }

            StudyStream requiredStream = GetStudyStream(ognp, streamNumber);
            Classes studyStreamClasses = requiredStream.Classes;
            DayOfWeek dayOfOgnp = requiredStream.DayOfWeek;
            GetGroupSchedule(new IsuGroupName(student.GroupName)).WeeklySchedule.CheckClassesAreNotIntersected(studyStreamClasses, dayOfOgnp);
            requiredStream.AddStudent(student);
        }

        public IReadOnlyList<Student> GetNonEnrolledStudents(Group group)
        {
            return GetGroup(group.GroupName).Students
                .Where(p => GetStudentOgnpsCount(p.Id) < NumberOfOgnp)
                .ToList();
        }

        public IReadOnlyList<Student> GetStudentsFromOgnp(Institute institute, StreamNumber streamNumber)
        {
            Ognp requiredOgnp = FindOgnp(institute.Name);
            if (requiredOgnp == null)
            {
                throw new IsuExtraException($"Can not find ognp from {institute.Name} school");
            }

            StudyStream requiredStream = requiredOgnp.FindStream(streamNumber);
            if (requiredStream == null)
            {
                throw new IsuExtraException($"Can not find stream {streamNumber.Number} from {institute.Name} school");
            }

            return requiredStream.Students;
        }

        public IReadOnlyList<StudyStream> GetStudyStreams(Institute institute)
        {
            Ognp requiredOgnp = FindOgnp(institute.Name);
            if (requiredOgnp == null)
            {
                throw new IsuExtraException($"Can not find ognp from {institute.Name} school");
            }

            return requiredOgnp.GetStudyStreams();
        }

        public void RemoveStudentsEnrollmentToOgnp(Student student, Ognp ognp, StreamNumber streamNumber)
        {
            GetStudyStream(ognp, streamNumber).RemoveStudent(student);
        }

        public int GetStudentOgnpsCount(int studentId)
        {
            return _ognps
                .Select(ognp => ognp.GetStudyStreams())
                .Count(p => p.Any(studyStream => studyStream.Contains(studentId)));
        }

        private void CheckGroupsAreNotDuplicated(List<Group> groups)
        {
            if (groups == null)
            {
                throw new IsuExtraException("Error: null reference. Group can not be a null");
            }

            var groupNames = groups
                .Select(p => p.GroupName)
                .ToList();
            var uniqueNames = groupNames
                .Distinct()
                .ToList();
            if (groupNames.Count != uniqueNames.Count)
            {
                throw new IsuExtraException("Error: Groups are not unique");
            }
        }

        private void CheckThatAllGroupsHaveSchedule(List<WeeklyScheduleForGroup> groupSchedule)
        {
            if (groupSchedule == null)
            {
                throw new IsuExtraException("Error: null reference. Group schedule can not be a null");
            }

            if (_groups
                .Any(p => groupSchedule.All(schedule => !schedule.GroupName.Name.Equals(p.GroupName.Name))))
            {
                throw new IsuExtraException("Error: Could not find schedule for group");
            }
        }

        private Group GetGroup(IsuGroupName name)
        {
            Group requiredGroup = _groups.FirstOrDefault(p => p.GroupName.Name.Equals(name.Name));
            if (requiredGroup == null)
            {
                throw new IsuExtraException($"Group with name {name.Name} does not exists");
            }

            return requiredGroup;
        }

        private Student FindStudent(int studentId)
        {
            return _groups
                .Select(group => group.FindStudent(studentId))
                .FirstOrDefault(findingStudent => findingStudent != null);
        }

        private Ognp FindOgnp(string name)
        {
            return _ognps.FirstOrDefault(p => p.Institute.Name.Equals(name));
        }

        private bool ContainsOgnp(string name)
        {
            return FindOgnp(name) != null;
        }

        private bool ContainsStudent(int studentId)
        {
            return FindStudent(studentId) != null;
        }

        private StudyStream GetStudyStream(Ognp ognp, StreamNumber streamNumber)
        {
            if (!ContainsOgnp(ognp.Institute.Name))
            {
                throw new IsuExtraException($"Ognp with name {ognp.Institute.Name} does not exists");
            }

            Ognp requiredOgnp = FindOgnp(ognp.Institute.Name);

            if (!requiredOgnp.ContainsStream(streamNumber))
            {
                throw new IsuExtraException(
                    $"Ognp from {ognp.Institute.Name} school does not contains stream with number {streamNumber.Number}");
            }

            StudyStream requiredStream = requiredOgnp.FindStream(streamNumber);
            return requiredStream;
        }

        private WeeklyScheduleForGroup GetGroupSchedule(IsuGroupName groupName)
        {
            WeeklyScheduleForGroup requiredSchedule =
                _groupsSchedule.FirstOrDefault(p => p.GroupName.Name.Equals(groupName.Name));
            if (requiredSchedule == null)
            {
                throw new IsuExtraException($"Group with name {groupName.Name} does not have schedule");
            }

            return requiredSchedule;
        }
    }
}