using System.Collections.Generic;
using System.Linq;
using Backups.Services;
using BackupsExtra.Tools;

namespace BackupsExtra.MergesAlgorithm
{
    public class MergeAlgorithm
    {
        private readonly IBackupJob _backupJob;
        private readonly RestorePoint _pastRestorePoint;
        private readonly RestorePoint _newRestorePoint;

        public MergeAlgorithm(IBackupJob backupJob, RestorePoint pastRestorePoint, RestorePoint newRestorePoint)
        {
            _backupJob = backupJob;
            _pastRestorePoint = pastRestorePoint;
            _newRestorePoint = newRestorePoint;
        }

        public void Execute()
        {
            List<RestorePoint> points = _backupJob.GetRestorePoints();
            RestorePoint pastPoint = points.FirstOrDefault(point => point.Equals(_pastRestorePoint));
            if (pastPoint == null)
            {
                throw new BackupsExtraException("Could not find restore point");
            }

            RestorePoint newPoint = points.FirstOrDefault(point => point.Equals(_newRestorePoint));
            if (newPoint == null)
            {
                throw new BackupsExtraException("Could not find restore point");
            }

            points.Remove(pastPoint);
            GetNewPoint(_pastRestorePoint, _newRestorePoint);
        }

        private void GetNewPoint(RestorePoint pastPoint, RestorePoint newPoint)
        {
            pastPoint.GetJobObjectStorages().ForEach(storage =>
            {
                IJobObject jobObject = storage.JobObject;
                bool existInNewPoint = newPoint.GetJobObjectStorages().Any(storages => storages.JobObject.GetObjectName().Equals(jobObject.GetObjectName()));
                if (!existInNewPoint)
                {
                    newPoint.AddStorage(storage);
                }
            });
        }
    }
}