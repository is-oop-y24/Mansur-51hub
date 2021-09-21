using System.Collections.Generic;
using System.Linq;
using Isu.Tools;

namespace Isu.Services
{
    public class Group
    {
        public Group(string name)
        {
            GroupName = new IsuGroupName(name);
            Students = new List<Student>();
        }

        public IsuGroupName GroupName { get; }

        public List<Student> Students { get; }

        public int MaxNumberOfStudents { get; } = 30;

        public CourseNumber GetCourse()
        {
            return GroupName.GetCourseNumber();
        }

        public IsuGroupNumber GetGroupNumber()
        {
            return GroupName.GetGroupNumber();
        }

        public Student FindStudent(string name)
        {
            Student student = Students.SingleOrDefault(p => p.Name.Equals(name));
            return student;
        }

        public Student FindStudent(int id)
        {
            Student student = Students.SingleOrDefault(p => p.Id.Equals(id));
            return student;
        }

        public Student FindStudent(Student student)
        {
            Student studentInGroup = Students.SingleOrDefault(p => p.Id.Equals(student.Id));
            return studentInGroup;
        }

        public Student AddStudent(Student student)
        {
            if (FindStudent(student) != null)
                throw new IsuException($"Student with name {student.Name} and id {student.Id} is already in group");
            if (Students.Count.Equals(MaxNumberOfStudents))
            {
                throw new IsuException(
                    $"Too many students in group {GroupName.Name}, maximal number is {MaxNumberOfStudents}");
            }

            Students.Add(student);
            return student;
        }

        public Student RemoveStudent(Student student)
        {
            Student removingStudent = FindStudent(student);
            if (removingStudent == null)
            {
                throw new IsuException(
                    $"There is no student with name {student.Name} and id {student.Id} in group {GroupName.Name}");
            }

            Students.Remove(removingStudent);
            return student;
        }
    }
}