using System.Collections.Generic;
using System.Linq;
using Backups.Services;

namespace BackupsExtra.PointCleaningAlgorithms
{
    public class ByTheAnyOfMixedAlgorithms : IPointsCleaningAlgorithm
    {
        private readonly List<IPointsCleaningAlgorithm> _algorithms;

        public ByTheAnyOfMixedAlgorithms(List<IPointsCleaningAlgorithm> algorithms)
        {
            _algorithms = algorithms;
        }

        public bool ShouldPointBeDeleted(RestorePoint point)
        {
            return _algorithms.Any(algorithm => algorithm.ShouldPointBeDeleted(point));
        }

        public void Execute()
        {
            _algorithms.ForEach(algorithm =>
            {
                algorithm.Execute();
            });
        }
    }
}