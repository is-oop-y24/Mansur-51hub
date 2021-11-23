namespace Backups.Services
{
    public class Storage
    {
        public Storage(JobObject jobObject)
        {
            JobObject = jobObject;
        }

        public JobObject JobObject { get; }

        public string GetMessageForLogger()
        {
            return $"Object name: {JobObject.GetObjectName()}";
        }
    }
}