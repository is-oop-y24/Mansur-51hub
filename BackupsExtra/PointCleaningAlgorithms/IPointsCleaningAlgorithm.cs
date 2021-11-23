using Backups.Services;

namespace BackupsExtra.PointCleaningAlgorithms
{
    public interface IPointsCleaningAlgorithm
    {
        bool ShouldPointBeDeleted(RestorePoint point);
        void Execute();
    }
}