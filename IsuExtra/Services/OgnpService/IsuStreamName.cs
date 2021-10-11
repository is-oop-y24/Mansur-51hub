namespace IsuExtra.Services.OgnpService
{
    public class IsuStreamName
    {
        public IsuStreamName(string name, StreamNumber streamNumber)
        {
            Name = name;
            StreamNumber = streamNumber;
        }

        public string Name { get; }
        public StreamNumber StreamNumber { get; }
    }
}