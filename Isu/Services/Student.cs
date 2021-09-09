namespace Isu.Services
{
    public class Student
    {
        public Student(int id, string name, string groupName)
        {
            Id = id;
            Name = name;
            GroupName = groupName;
        }

        public int Id { get; }

        public string Name { get; set; }

        public string GroupName { get; }
    }
}