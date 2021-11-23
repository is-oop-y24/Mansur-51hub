using System;

namespace BackupsExtra.Configurations
{
    public class JobConfiguration
    {
        public JobConfiguration(string name, string rootPath, int actualPointsCount)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            RootPath = rootPath ?? throw new ArgumentNullException(nameof(rootPath));
            ActualPointsCount = actualPointsCount;
        }

        public string Name { get; }
        public string RootPath { get; }
        public int ActualPointsCount { get; }
    }
}