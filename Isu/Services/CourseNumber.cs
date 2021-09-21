using Isu.Tools;

namespace Isu.Services
{
    public class CourseNumber
    {
        private const int FirstYear = 1;
        private const int LastYear = 4;

        public CourseNumber(int number)
        {
            if (!IsCorrectNumber(number))
            {
                throw new IsuException($"Course number out of range. Range: from {FirstYear} to {LastYear}");
            }

            Number = number;
        }

        public int Number { get; }

        public static bool IsCorrectNumber(int number)
        {
            return number is >= FirstYear and <= LastYear;
        }
    }
}