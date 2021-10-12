using System.Collections.Generic;
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

            string beginning = name[..2];
            var templates = new List<string>()
            {
                "N3",
                "P3",
                "R3",
                "H3",
                "W3",
                "O3",
                "B3",
                "T3",
                "L3",
                "Z3",
                "K3",
                "M3",
            };

            if (templates.TrueForAll(p => !p.Equals(beginning)))
            {
                throw new IsuException($"Unexpected start of group name");
            }

            int courseNumber = int.Parse(name.Substring(2, 1));
            _courseNumber = new CourseNumber(courseNumber);

            int groupNumber = int.Parse(name.Substring(3, 2));
            _groupNumber = new IsuGroupNumber(groupNumber);
        }
    }
}