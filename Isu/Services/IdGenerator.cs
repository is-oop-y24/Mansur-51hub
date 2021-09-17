namespace Isu.Services
{
    public class IdGenerator
    {
        private int _serialNumber = 0;

        public int GenerateId()
        {
            _serialNumber++;
            return _serialNumber;
        }
    }
}