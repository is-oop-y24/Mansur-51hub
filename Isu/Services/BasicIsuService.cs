using System.Collections.Generic;
using System.Linq;
using Isu.Tools;

namespace Isu.Services
{
    public class BasicIsuService : IIsuService
    {
        private readonly List<Group> _groups;
        private readonly IdGenerator _id;

        public BasicIsuService()
        {
            _groups = new List<Group>();
            _id = new IdGenerator();
        }

        public BasicIsuService(List<Group> groups)
        {
            _groups = new List<Group>(groups);
            _id = new IdGenerator();
        }

        public Group AddGroup(string name)
        {
            var group = new Group(name);

            if (FindGroup(@group.GroupName.Name) != null)
            {
                throw new IsuException($"Group with name {name} already exists");
            }

            _groups.Add(new Group(name));
            return @group;
        }

        public Student AddStudent(Group group, string name)
        {
            Group expectedGroup = FindGroup(group.GroupName.Name);
            if (expectedGroup == null)
            {
                throw new IsuException($"No such group in ISU: {group.GroupName.Name}");
            }

            var student = new Student(_id.GenerateId(), name, group.GroupName.Name);
            return expectedGroup.AddStudent(student);
        }

        public Group FindGroup(string groupName)
        {
            Group group = _groups.SingleOrDefault(p => p.GroupName.Name.Equals(groupName));
            return group;
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            return _groups
                         .Where(p => p.GetCourse().Number == courseNumber.Number)
                         .ToList();
        }

        public Student FindStudent(int id)
        {
            return _groups
                .Select(@group => @group.FindStudent(id))
                .FirstOrDefault(findingStudent => findingStudent != null);
        }

        public Student FindStudent(string name)
        {
            return _groups
                .Select(@group => @group.FindStudent(name))
                .FirstOrDefault(findingStudent => findingStudent != null);
        }

        public List<Student> FindStudents(string groupName)
        {
            List<Student> students = FindGroup(groupName).Students;
            return students;
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            IEnumerable<Group> groups = _groups.Where(p => p.GetCourse().Number.Equals(courseNumber.Number));
            var students = groups.SelectMany(@group => @group.Students).ToList();
            return students;
        }

        public Student GetStudent(int id)
        {
            foreach (Group group in _groups)
            {
                if (group.FindStudent(id) != null)
                {
                    return group.FindStudent(id);
                }
            }

            throw new IsuException($"There is no student with id: {id} in Isu");
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            if (FindStudent(student.Id) == null)
            {
                throw new IsuException($"Student with name {student.Name} and id {student.Id} does not exists");
            }

            Group findingGroup = FindGroup(newGroup.GroupName.Name);

            if (findingGroup == null)
            {
                throw new IsuException($"Group with name {newGroup.GroupName.Name} does not exists");
            }

            findingGroup.AddStudent(new Student(student.Id, student.Name, newGroup.GroupName.Name));
            FindGroup(student.GroupName).RemoveStudent(student);
        }

        public IReadOnlyList<Group> GetGroups()
        {
            return _groups;
        }
    }
}