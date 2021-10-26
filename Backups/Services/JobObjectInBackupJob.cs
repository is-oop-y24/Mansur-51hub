namespace Backups.Services
{
    public class JobObjectInBackupJob
    {
        public JobObjectInBackupJob(int id, IJobObject jobObject)
        {
            Id = id;
            JobObject = jobObject;
        }

        public int Id { get; }
        public IJobObject JobObject { get; }
    }
}