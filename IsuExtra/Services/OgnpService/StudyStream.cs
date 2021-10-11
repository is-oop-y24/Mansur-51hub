using System.Collections.Generic;
using System.Linq;
using Isu.Services;
using Isu.Tools;
using IsuExtra.Services.ScheduleService;
using IsuExtra.Services.Tools;

namespace IsuExtra.Services.OgnpService
{
    public class StudyStream
    {
        public StudyStream(string name, StreamNumber streamNumber, Classes classes, DayOfWeek dayOfWeek)
        {
            StreamName = new IsuStreamName(name, streamNumber);
            Students = new List<Student>();
            Classes = classes;
            DayOfWeek = dayOfWeek;
        }

        public IsuStreamName StreamName { get; }

        public List<Student> Students { get; }

        public Classes Classes { get; }

        public DayOfWeek DayOfWeek { get; }

        public int MaxNumberOfStudents { get; } = 30;

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

        public bool Contains(int studentId)
        {
            return FindStudent(studentId) != null;
        }

        public Student AddStudent(Student student)
        {
            if (FindStudent(student) != null)
                throw new IsuException($"Student with name {student.Name} and id {student.Id} is already in group");
            if (Students.Count.Equals(MaxNumberOfStudents))
            {
                throw new IsuExtraException(
                    $"Too many students in group {StreamName.Name}, maximal number is {MaxNumberOfStudents}");
            }

            Students.Add(student);
            return student;
        }

        public Student RemoveStudent(Student student)
        {
            Student removingStudent = FindStudent(student);
            if (removingStudent == null)
            {
                throw new IsuExtraException(
                    $"There is no student with name {student.Name} and id {student.Id} in group {StreamName.Name}");
            }

            Students.Remove(removingStudent);
            return student;
        }
    }
}