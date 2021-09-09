using Isu.Tools;

namespace Isu.Services
{
    public class CourseNumber : IsuNumber
    {
        public CourseNumber(int number)
        {
            _ = IsCorrectNumber(number);
            Number = number;
        }

        public sealed override bool IsCorrectNumber(int number)
        {
            const int firstYear = 1;
            const int lastYear = 4;

            if (number is >= firstYear and <= lastYear)
                return true;
            throw new IsuException($"Course number out of range. Range: from {firstYear} to {lastYear}");
        }
    }
}