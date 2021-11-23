using System;
using System.Xml.Linq;
using BackupsExtra.Tools;

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

        public JobObjectConfiguration(XElement jobObject)
        {
            if (jobObject.FirstAttribute == null)
            {
                throw new BackupsExtraException("Incorrect configuration file: could not find job object name");
            }

            Name = jobObject.FirstAttribute.Value;
            CheckValueExist(jobObject, "JobPath");
            Path = jobObject.Element("JobPath").FirstAttribute?.Value;
            CheckValueExist(jobObject, "ZipPath");
            ZipPath = jobObject.Element("ZipPath").FirstAttribute?.Value;
            CheckValueExist(jobObject, "OriginalPath");
            OriginalPath = jobObject.Element("OriginalPath").FirstAttribute?.Value;
        }

        public string Name { get; }
        public string Path { get; }
        public string ZipPath { get; }
        public string OriginalPath { get; }

        private void CheckValueExist(XElement element, string expandedName)
        {
            if (element.Element(expandedName) == null)
            {
                throw new BackupsExtraException("Incorrect configuration file");
            }
        }
    }
}