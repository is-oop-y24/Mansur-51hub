using System.Collections.Generic;

namespace Backups.Services
{
    public interface IAlgorithmForCreatingBackups
    {
        void CreateRestorePoint(List<JobObjectInBackupJob> jobObjects, IRepository repository, int index, string jobDirectory);
        void CreateRestorePointExtra(List<JobObjectInBackupJob> jobObjects, IRepository repository, string jobDirectory, string rootDirectory, string jobName, int pointsCount);
    }
}