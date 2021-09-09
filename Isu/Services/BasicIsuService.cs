using System.Collections.Generic;
using System.Linq;
using Isu.Tools;

namespace Isu.Services
{
    public class BasicIsuService : IIsuService
    {
        private readonly List<Group> _groups;
        private readonly Id _id;

        public BasicIsuService()
        {
            _groups = new List<Group>();
            _id = new Id();
        }

        public BasicIsuService(List<Group> groups)
        {
            _groups = new List<Group>(groups);
            _id = new Id();
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
            Group gGroup = FindGroup(group.GroupName.Name);
            if (gGroup == null)
            {
                throw new IsuException($"No such group in ISU: {group.GroupName.Name}");
            }

            var student = new Student(_id.GetId(), name, group.GroupName.Name);
            return gGroup.AddStudent(student);
        }

        public Group FindGroup(string groupName)
        {
            Group group = _groups.SingleOrDefault(p => p.GroupName.Name.Equals(groupName));
            return group;
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            IEnumerable<Group> groups = _groups.Where(p => p.GetCourse().Number ==
                                                           courseNumber.Number);
            return groups.ToList();
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
            return (from @group in _groups where @group.FindStudent(id) != null select @group.FindStudent(id)).FirstOrDefault();
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
    }
}