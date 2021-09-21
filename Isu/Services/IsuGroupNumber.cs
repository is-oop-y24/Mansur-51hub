using Isu.Tools;

namespace Isu.Services
{
    public class IsuGroupNumber
    {
        private const int MinGroupNumber = 1;
        private const int MaxGroupNumber = 12;
        public IsuGroupNumber(int number)
        {
            if (!IsCorrectNumber(number))
            {
                throw new IsuException($"Group number out of range. Range: from {MinGroupNumber} to {MaxGroupNumber}");
            }

            Number = number;
        }

        public int Number { get; }

        public static bool IsCorrectNumber(int number)
        {
            return number is >= MinGroupNumber and <= MaxGroupNumber;
        }
    }
}