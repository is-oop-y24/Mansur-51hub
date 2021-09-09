namespace Isu.Services
{
    public class Id
    {
        private int _serialNumber = 0;

        public int GetId()
        {
            _serialNumber++;
            return _serialNumber;
        }
    }
}