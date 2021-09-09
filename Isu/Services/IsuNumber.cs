namespace Isu.Services
{
    public abstract class IsuNumber
    {
        private int _number;
        public int Number
        {
            get => _number;

            set
            {
                _ = IsCorrectNumber(value);
                _number = value;
            }
        }

        public abstract bool IsCorrectNumber(int number);
    }
}