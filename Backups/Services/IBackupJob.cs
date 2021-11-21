using System.Collections.Generic;

namespace Backups.Services
{
    public interface IBackupJob
    {
        void CreateRestorePointInRepository(IAlgorithmForCreatingBackups algorithm, IRepository repository);
        void CreateRestorePoint();
        void CreateRestorePoint(RestorePoint newRestorePoint);
        void AddObject(JobObject jobObject);
        void DeleteObject(int jobObjectId);
        List<RestorePoint> GetRestorePoints();
        IReadOnlyList<JobObjectInBackupJob> GetJobObjects();
        string GetJobName();
    }
}