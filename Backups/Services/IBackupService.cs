namespace Backups.Services
{
    public interface IBackupService
    {
        void CreateBackupJob(string jobName);
        IBackupJob GetBackupJob(string jobName);
    }
}