using System.Collections.Generic;

namespace Backups.Services
{
    public interface IBackupJob
    {
        void CreateRestorePoint(IAlgorithmForCreatingBackups algorithm, IRepository repository);
        void AddObject(IJobObject jobObject);
        void DeleteObject(int jobObjectId);
        IReadOnlyList<RestorePoint> GetRestorePoints();
        public IReadOnlyList<JobObjectInBackupJob> GetJobObjects();
        string GetJobName();
    }
}