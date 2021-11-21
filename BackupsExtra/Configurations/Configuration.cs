using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Backups.Services;
using BackupsExtra.Tools;

namespace BackupsExtra.Configurations
{
    public class Configuration
    {
        private readonly XDocument _configDocument;

        public Configuration(string rootPath)
        {
            _configDocument = new XDocument();
            MakeRoot(rootPath);
            _configDocument.Save($@"{SystemFileBuffer.DirectoryName}\{rootPath}\{ConfigurationFileName}");
        }

        public Configuration(string path, IRepository repository)
        {
            byte[] bytes = repository.GetBytes(path);
            CreateConfigurationFileInBuffer(bytes, @$"{SystemFileBuffer.DirectoryName}\{path}");
            try
            {
                _configDocument = XDocument.Load(@$"{SystemFileBuffer.DirectoryName}\{path}");
            }
            catch (Exception e)
            {
                throw new BackupsExtraException(e.Message);
            }
        }

        public static string ConfigurationFileName { get; } = "config.xml";

        public void AddToConfiguration(JobConfiguration jobConfiguration, RestorePointConfiguration restorePoint)
        {
            XElement existingJob = _configDocument.Root.Elements("Job").FirstOrDefault(job => job.FirstAttribute.Value.Equals(jobConfiguration.Name));
            XElement job;
            if (existingJob == null)
            {
                job = new XElement("Job");
                var jobNameInXml = new XAttribute("name", jobConfiguration.Name);
                var jobPointsCount = new XAttribute("PointsCount", jobConfiguration.ActualPointsCount);
                job.Add(jobNameInXml);
                job.Add(jobPointsCount);
            }
            else
            {
                job = existingJob;
                job.Attribute("PointsCount").Value = jobConfiguration.ActualPointsCount.ToString();
            }

            var restorePointInXml = new XElement("RestorePoint");
            var creationTime = new XAttribute("CreationTime", $"{restorePoint.CreationTime:yy-MM-dd}");
            restorePointInXml.Add(creationTime);
            restorePoint.GetJobObjects().ToList().ForEach(jobObjectConfiguration =>
            {
                var jobObject = new XElement("JobObject");
                var jobObjectName = new XAttribute("name", jobObjectConfiguration.Name);
                var jobObjectPath = new XElement("JobPath");
                var jobDirectoryPath = new XAttribute("path", jobObjectConfiguration.Path);
                var jobObjectZipPath = new XElement("ZipPath");
                var jobZipPath = new XAttribute("path", jobObjectConfiguration.ZipPath);
                var jobObjectOriginalPath = new XElement("OriginalPath");
                var jobOriginalPath = new XAttribute("path", jobObjectConfiguration.OriginalPath);

                jobObject.Add(jobObjectName);
                jobObjectPath.Add(jobDirectoryPath);
                jobObjectZipPath.Add(jobZipPath);
                jobObjectOriginalPath.Add(jobOriginalPath);

                jobObject.Add(jobObjectPath);
                jobObject.Add(jobObjectZipPath);
                jobObject.Add(jobObjectOriginalPath);
                restorePointInXml.Add(jobObject);
            });

            job.Add(restorePointInXml);

            if (job != existingJob)
            {
                _configDocument.Root?.Add(job);
            }

            _configDocument.Save(@$"C:\backups\{jobConfiguration.RootPath}\{ConfigurationFileName}");
        }

        public byte[] GetBytesData(string rootPath)
        {
            string configurationPatnInBuffer = @$"C:\backups\{rootPath}\{ConfigurationFileName}";
            var fileInfo = new FileInfo(configurationPatnInBuffer);
            if (!fileInfo.Exists)
            {
                throw new BackupsExtraException("Could not find configuration time");
            }

            using FileStream fstream = File.OpenRead(configurationPatnInBuffer);
            byte[] bytes = new byte[fstream.Length];
            fstream.Read(bytes, 0, bytes.Length);
            fstream.Close();
            return bytes;
        }

        private void CreateConfigurationFileInBuffer(byte[] bytes, string path)
        {
            using FileStream fstream = File.Create(path, bytes.Length);
            fstream.Write(bytes, 0, bytes.Length);
            fstream.Close();
        }

        private void MakeRoot(string rootPath)
        {
            var root = new XElement("Repository");
            var attribute = new XAttribute("path", rootPath);
            root.Add(attribute);
            _configDocument.Add(root);
        }
    }
}