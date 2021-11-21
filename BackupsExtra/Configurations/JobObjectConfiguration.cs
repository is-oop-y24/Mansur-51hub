using System;

namespace BackupsExtra.Configurations
{
    public class JobObjectConfiguration
    {
        public JobObjectConfiguration(string name, string path, string zipPath, string originalPath)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Path = path ?? throw new ArgumentNullException(nameof(path));
            ZipPath = zipPath ?? throw new ArgumentNullException(nameof(zipPath));
            OriginalPath = originalPath ?? throw new ArgumentNullException(nameof(originalPath));
        }

        public string Name { get; }
        public string Path { get; }
        public string ZipPath { get; }
        public string OriginalPath { get; }
    }
}