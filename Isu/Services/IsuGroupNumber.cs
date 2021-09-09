using Isu.Tools;

namespace Isu.Services
{
    public class IsuGroupNumber : IsuNumber
    {
        public IsuGroupNumber(int number)
        {
            _ = IsCorrectNumber(number);
            Number = number;
        }

        public sealed override bool IsCorrectNumber(int number)
        {
            const int minGroupNumber = 0;
            const int maxGroupNumber = 12;

            if (number is >= minGroupNumber and <= maxGroupNumber)
            {
                return true;
            }

            throw new IsuException($"Group number out of range. Range: from {minGroupNumber} to {maxGroupNumber}");
        }
    }
}