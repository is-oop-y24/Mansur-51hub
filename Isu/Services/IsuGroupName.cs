using Isu.Tools;

namespace Isu.Services
{
    public class IsuGroupName
    {
        private CourseNumber _courseNumber;
        private IsuGroupNumber _groupNumber;

        public IsuGroupName(string name)
        {
            ParseString(name);
            Name = name;
        }

        public string Name { get; }
        public CourseNumber GetCourseNumber()
        {
            return _courseNumber;
        }

        public IsuGroupNumber GetGroupNumber()
        {
            return _groupNumber;
        }

        private void ParseString(string name)
        {
            const int groupNameLength = 5;
            if (name.Length != groupNameLength)
            {
                throw new IsuException(
                    $"Unexpected length of group name. Expected: {groupNameLength}, Got: {name.Length}");
            }

            const string template = "M3";
            string beginning = name[..2];
            if (!template.Equals(beginning))
            {
                throw new IsuException($"Unexpected start of group name. Expected: {template}, Got: {name[..2]}");
            }

            int courseNumber = int.Parse(name.Substring(2, 1));
            _courseNumber = new CourseNumber(courseNumber);

            int groupNumber = int.Parse(name.Substring(3, 2));
            _groupNumber = new IsuGroupNumber(groupNumber);
        }
    }
}