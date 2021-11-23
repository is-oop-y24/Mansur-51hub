using System.Collections.Generic;
using System.Linq;
using Backups.Services;
using BackupsExtra.Tools;

namespace BackupsExtra.PointCleaningAlgorithms
{
    public class ByTheNumberOfPointsAlgorithm : IPointsCleaningAlgorithm
    {
        private readonly IBackupJob _backupJob;
        private readonly int _pointsNumber;

        public ByTheNumberOfPointsAlgorithm(IBackupJob backupJob, int pointsNumber)
        {
            _backupJob = backupJob;
            _pointsNumber = pointsNumber;
        }

        public bool ShouldPointBeDeleted(RestorePoint point)
        {
            RestorePoint restorePoint = _backupJob.GetRestorePoints().FirstOrDefault(points => points.Equals(point));
            if (restorePoint == null)
            {
                return false;
            }

            int index = _backupJob.GetRestorePoints().IndexOf(point);
            return _backupJob.GetRestorePoints().Count > _pointsNumber && index < _backupJob.GetRestorePoints().Count - _pointsNumber;
        }

        public void Execute()
        {
            List<RestorePoint> points = _backupJob.GetRestorePoints();
            if (points.Count > _pointsNumber)
            {
                points.RemoveRange(0, points.Count - _pointsNumber);
                if (points.Count.Equals(0))
                {
                    throw new BackupsExtraException("It is prohibited to delete all restore points");
                }
            }
        }
    }
}