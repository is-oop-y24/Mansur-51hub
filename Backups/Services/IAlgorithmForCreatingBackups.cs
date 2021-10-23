using System.Collections.Generic;

namespace Backups.Services
{
    public interface IAlgorithmForCreatingBackups
    {
        RestorePoint CreateRestorePoint(List<JobObjectInBackupJob> jobObjects, IRepository repository, int index, string jobDirectory);
    }
}