namespace Backups.Services
{
    public class Storage
    {
        public Storage(IJobObject jobObject)
        {
            JobObject = jobObject;
        }

        public IJobObject JobObject { get; }
    }
}