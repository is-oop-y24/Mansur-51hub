using System.Collections.Generic;

namespace Backups.Services
{
    public interface IBackupService
    {
        void CreateBackupJob(IBackupJob backupJob);
        IBackupJob GetBackupJob(string jobName);
        List<IBackupJob> GetAllBackupJobs();
        string RootDirectory();
    }
}