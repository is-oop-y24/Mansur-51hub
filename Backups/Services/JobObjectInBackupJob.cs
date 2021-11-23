namespace Backups.Services
{
    public class JobObjectInBackupJob
    {
        public JobObjectInBackupJob(int id, JobObject jobObject)
        {
            Id = id;
            JobObject = jobObject;
        }

        public int Id { get; }
        public JobObject JobObject { get; }
    }
}