using System.Collections.Generic;

namespace Backups.Services
{
    public interface IBackupJob
    {
        void CreateRestorePoint(IAlgorithmForCreatingBackups algorithm);
        void AddObject(IJobObject jobObject);
        void DeleteObject(int jobObjectId);
        IReadOnlyList<RestorePoint> GetRestorePoints();
        public IReadOnlyList<JobObjectInBackupJob> GetJobObjects();
        string GetJobName();
    }
}