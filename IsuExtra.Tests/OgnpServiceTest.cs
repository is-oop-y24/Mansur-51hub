using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Services;
using IsuExtra.Services.OgnpService;
using IsuExtra.Services.ScheduleService;
using IsuExtra.Services.Tools;
using NUnit.Framework;

namespace IsuExtra.Tests
{
    public class OgnpServiceTest
    {
        private IOgnpService _ognpService;
        private IScheduleService _scheduleService;
        private IIsuService _isuService;

        [SetUp]
        public void SetUp()
        {
            _isuService = new BasicIsuService();
            _scheduleService = new BasicScheduleService();

            _isuService.AddGroup("M3201");
            _isuService.AddGroup("M3202");
            for (int i = 1; i < 40; i++)
            {
                _isuService.AddStudent(i < 20 ? new Group("M3201") : new Group("M3202"), $"student {i}");
            }
            var mondaySchedule = new DailySchedule(DayOfWeek.Monday);
            mondaySchedule.AddClasses(new Classes("OOP", new Tutor("Tutor1", 1), new Room("234", new Address("Kronva", "23")), new Time(10, 00), new Time(11, 30)));
            var fridaySchedule = new DailySchedule(DayOfWeek.Friday);
            fridaySchedule.AddClasses(new Classes("History", new Tutor("Tutor2", 2), new Room("456", new Address("Kronva", "23")), new Time(8, 20), new Time(9, 50)));
            var scheduleForM3200_01 = new List<DailySchedule>()
            {
                mondaySchedule,
                fridaySchedule,
            };

            _scheduleService.SetGroupSchedule(new IsuGroupName("M3201"), new WeeklySchedule(new List<DailySchedule>(scheduleForM3200_01)));
            _scheduleService.SetGroupSchedule(new IsuGroupName("M3202"), new WeeklySchedule(new List<DailySchedule>(scheduleForM3200_01)));

            _ognpService = new BasicOgnpService(_isuService, _scheduleService);
        }

        [Test]
        public void AddNewOgnp_OgnpWasAddedBefore_ThrowException()
        {
            Assert.Catch<IsuExtraException>(() =>
            {
                _ognpService.AddNewOgnp(new Ognp("Kib", new ItmoInstitutes().GetInstitutes()[0]));
                _ognpService.AddNewOgnp(new Ognp("Kib", new ItmoInstitutes().GetInstitutes()[0]));
            });
        }

        [Test]
        public void AddStudentToHisSchoolOgnp_ThrowException()
        {
            var ognp = new Ognp("Kt",
                new ItmoInstitutes().GetInstitutes()
                    .First(p => p.Name.Equals("School of Translational Information Technologies")));
            ognp.AddStudyStream(new StudyStream("First sream", new StreamNumber(1), new Classes("OOP", new Tutor("Tutor1", 1), new Room("234", new Address("Kronva", "23")), new Time(10, 00), new Time(11, 30)), DayOfWeek.Wednesday));
            _ognpService.AddNewOgnp(ognp);
            Assert.Catch<IsuExtraException>(() => {
               _ognpService.EnrollStudentToOgnp(new Student(2, "student 2", "M3201"), ognp, new StreamNumber(1));
            });
        }

        [Test]
        public void EnrollStudentToOgnp_TimesAreIntersected_ThrowException()
        {
            var ognp = new Ognp("Kt",
                new ItmoInstitutes().GetInstitutes()[0]);
            ognp.AddStudyStream(new StudyStream("First stream", new StreamNumber(1), new Classes("Lection", new Tutor("Tutor1", 1), new Room("234", new Address("Kronva", "23")), new Time(10, 30), new Time(12, 00)), DayOfWeek.Monday));
            _ognpService.AddNewOgnp(ognp);
            Assert.Catch<IsuExtraException>(() => {
                _ognpService.EnrollStudentToOgnp(new Student(2, "student 2", "M3201"), ognp, new StreamNumber(1));
            });
        }

        [Test]
        public void AddStudentsToStream_TooManyStudents_ThrowException()
        {
            var ognp = new Ognp("Kt",
                new ItmoInstitutes().GetInstitutes()[0]);
            ognp.AddStudyStream(new StudyStream("First stream", new StreamNumber(1), new Classes("Lection", new Tutor("Tutor1", 1), new Room("234", new Address("Kronva", "23")), new Time(10, 30), new Time(12, 00)), DayOfWeek.Wednesday));
            _ognpService.AddNewOgnp(ognp);
            Assert.Catch<IsuExtraException>(() => {
                for (int i = 1; i < 40; i++)
                {
                    _ognpService.EnrollStudentToOgnp(new Student(i, $"student {i}", i < 20 ? "M3201" : "M3202"), ognp, new StreamNumber(1));
                }
            });
        }

        [Test]
        public void RemoveStudentsEnrollmentToOgnp_SuccessfullyRemoved()
        {
            var ognp = new Ognp("Kt",
                new ItmoInstitutes().GetInstitutes()[0]);
            ognp.AddStudyStream(new StudyStream("First stream", new StreamNumber(1), new Classes("Lection", new Tutor("Tutor1", 1), new Room("234", new Address("Kronva", "23")), new Time(10, 30), new Time(12, 00)), DayOfWeek.Wednesday));
            _ognpService.AddNewOgnp(ognp);
            _ognpService.EnrollStudentToOgnp(new Student(2, "student 2", "M3201"), ognp, new StreamNumber(1));
            _ognpService.RemoveStudentsEnrollmentToOgnp(new Student(2, "student 2", "M3201"), ognp, new StreamNumber(1));
            Assert.Zero(_ognpService.GetStudentOgnpsCount(2));
        }

        [Test]
        public void GetNonEnrolledStudents_SuccessfullyGet()
        {
            var ognp = new Ognp("Kt",
                new ItmoInstitutes().GetInstitutes()[0]);
            ognp.AddStudyStream(new StudyStream("First stream", new StreamNumber(1), new Classes("Lection", new Tutor("Tutor1", 1), new Room("234", new Address("Kronva", "23")), new Time(10, 30), new Time(12, 00)), DayOfWeek.Wednesday));
            ognp.AddStudyStream(new StudyStream("First stream", new StreamNumber(2), new Classes("Lection", new Tutor("Tutor1", 1), new Room("234", new Address("Kronva", "23")), new Time(11, 40), new Time(13, 10)), DayOfWeek.Wednesday));
            _ognpService.AddNewOgnp(ognp);
            
            for (int i = 1; i < 38; i++)
            {
                if (i < 20)
                {
                    _ognpService.EnrollStudentToOgnp(new Student(i, $"student {i}", i < 20 ? "M3201" : "M3202"), ognp,
                        new StreamNumber(1));
                }
                else
                {
                    _ognpService.EnrollStudentToOgnp(new Student(i, $"student {i}","M3202"), ognp, new StreamNumber(2));
                }
            }

            var nonEnrolledStudents = _ognpService.GetNonEnrolledStudents(new Group("M3202")).ToList();
            Assert.AreEqual(2, nonEnrolledStudents.Count);
        }
    }
}