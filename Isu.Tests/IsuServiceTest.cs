using System;
using Isu.Services;
using Isu.Tools;
using NUnit.Framework;

namespace Isu.Tests
{
    public class Tests
    {
        private IIsuService _isuService;

        [SetUp]
        public void Setup()
        {
            _isuService = new BasicIsuService();
        }

        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            try
            {
                _isuService.AddGroup("M3201");
                _isuService.AddStudent(new Group("M3201"), "student");
                Student student = new Student(1, "student", "M3201");
                _isuService.ChangeStudentGroup(student, new Group("M3201"));
                Assert.Fail();
            }

            catch (IsuException)
            {
            }
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                Group group = new Group("M3201");
                _isuService.AddGroup("M3201");
                for (int i = 0; i < 31; i++)
                {
                    _isuService.AddStudent(group, i.ToString());
                }
            });
        }

        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                Group group = new Group("M32011");
            });
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            Assert.Catch<IsuException>(() =>
            {
                _isuService.AddGroup("M3201");
                _isuService.AddGroup("M3301");
                Group newGroup = new Group("M3301");
                _isuService.AddStudent(new Group("M3201"), "student");
                Student student = new Student(1, "student", "M3201");
                _isuService.ChangeStudentGroup(student, newGroup);
                _isuService.FindGroup("M3201").RemoveStudent(student);
            });
        }
    }
}