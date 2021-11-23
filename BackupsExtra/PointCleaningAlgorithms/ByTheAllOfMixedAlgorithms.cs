using System.Collections.Generic;
using System.Linq;
using Backups.Services;

namespace BackupsExtra.PointCleaningAlgorithms
{
    public class ByTheAllOfMixedAlgorithms : IPointsCleaningAlgorithm
    {
        private readonly List<IPointsCleaningAlgorithm> _algorithms;
        private readonly IBackupJob _backupJob;

        public ByTheAllOfMixedAlgorithms(IBackupJob backupJob, List<IPointsCleaningAlgorithm> algorithms)
        {
            _backupJob = backupJob;
            _algorithms = algorithms;
        }

        public bool ShouldPointBeDeleted(RestorePoint point)
        {
            return _algorithms.All(algorithm => algorithm.ShouldPointBeDeleted(point));
        }

        public void Execute()
        {
            _backupJob.GetRestorePoints().RemoveAll(ShouldPointBeDeleted);
        }
    }
}