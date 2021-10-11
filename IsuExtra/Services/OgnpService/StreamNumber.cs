using IsuExtra.Services.Tools;

namespace IsuExtra.Services.OgnpService
{
    public class StreamNumber
    {
        private const int _minStreamNumber = 1;
        private const int _maxStreamNumber = 5;

        public StreamNumber(int number)
        {
            if (number < _minStreamNumber || number > _maxStreamNumber)
            {
                throw new IsuExtraException(
                    $"Error stream number. Expected in range {_minStreamNumber} to {_maxStreamNumber}");
            }

            Number = number;
        }

        public int Number { get; }
    }
}