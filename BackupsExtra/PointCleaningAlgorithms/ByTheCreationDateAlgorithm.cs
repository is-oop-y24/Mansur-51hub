using System;
using System.Collections.Generic;
using Backups.Services;
using BackupsExtra.Tools;

namespace BackupsExtra.PointCleaningAlgorithms
{
    public class ByTheCreationDateAlgorithm : IPointsCleaningAlgorithm
    {
        private readonly IBackupJob _backupJob;
        private readonly DateTime _date;

        public ByTheCreationDateAlgorithm(IBackupJob backupJob, DateTime date)
        {
            _backupJob = backupJob;
            _date = date;
        }

        public bool ShouldPointBeDeleted(RestorePoint point)
        {
            return point.Date.CompareTo(_date) < 0;
        }

        public void Execute()
        {
            List<RestorePoint> points = _backupJob.GetRestorePoints();
            if (points.Count == 0) return;
            points.RemoveAll(point => point.Date.CompareTo(_date) < 0);

            if (points.Count.Equals(0))
            {
                throw new BackupsExtraException("It is prohibited to delete all restore points");
            }
        }
    }
}