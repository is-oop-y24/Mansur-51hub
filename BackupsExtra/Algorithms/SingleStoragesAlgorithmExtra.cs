using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Backups.Services;
using BackupsExtra.Configurations;

namespace BackupsExtra.Algorithms
{
    public class SingleStoragesAlgorithmExtra : IAlgorithmForCreatingBackups
    {
        private string _restorePointDirectory;
        private string _restorePointFullDirectory;
        private Configuration _configuration;
        private JobConfiguration _jobConfiguration;

        public void CreateRestorePoint(List<JobObjectInBackupJob> jobObjects, IRepository repository, int index, string jobDirectory)
        {
            new SingleStoragesAlgorithm().CreateRestorePoint(jobObjects, repository, index, jobDirectory);
        }

        public void CreateRestorePointExtra(List<JobObjectInBackupJob> jobObjects, IRepository repository, string jobDirectory, string rootDirectory, string jobName, int pointsCount)
        {
            var fileBuffer = new SystemFileBuffer();
            repository.CreateDirectory(jobDirectory);
            fileBuffer.CreateDirectory(jobDirectory);
            _jobConfiguration = new JobConfiguration(jobName, rootDirectory, pointsCount);

            GetConfigurationFile(rootDirectory, repository);

            int index = new GetIndexAlgorithm().GetIndex(repository, jobDirectory);

            MakeZipObject(jobObjects, fileBuffer, index, jobDirectory);

            repository.SaveFiles(_restorePointFullDirectory, _restorePointDirectory);

            string configurationDirectoryInRepository = @$"{rootDirectory}\{Configuration.ConfigurationFileName}";
            repository.CreateFile(_configuration.GetBytesData(rootDirectory), configurationDirectoryInRepository);

            fileBuffer.Clear();
        }

        private void MakeZipObject(List<JobObjectInBackupJob> jobObjects, SystemFileBuffer fileBuffer, int index, string jobDirectory)
        {
            _restorePointDirectory = @$"{jobDirectory}\Restore_point_{index}";
            _restorePointFullDirectory = $@"{SystemFileBuffer.DirectoryName}\{_restorePointDirectory}";
            fileBuffer.CreateDirectory(_restorePointDirectory);

            string objectsDirectory = @$"{SystemFileBuffer.DirectoryName}\{jobDirectory}\Objects";
            fileBuffer.CreateDirectory($@"{jobDirectory}\Objects");

            var restorePoint = new RestorePointConfiguration(DateTime.Now);

            jobObjects.ForEach(jobObject =>
            {
                string jobObjectName = jobObject.JobObject.GetObjectName();
                byte[] dataBytes = jobObject.JobObject.GetBytes().ToArray();

                string path = $@"{objectsDirectory}\{jobObjectName}_{index}";

                string pathInRepository = @$"{_restorePointDirectory}\Objects.zip\{jobObjectName}_{index}";
                var jobObjectConfiguration = new JobObjectConfiguration(jobObjectName, pathInRepository, $@"{_restorePointDirectory}\Objects.zip", jobObject.JobObject.OriginalPath);
                restorePoint.AddJobObject(jobObjectConfiguration);

                using FileStream fstream = File.Create(path, dataBytes.Length);
                fstream.Write(dataBytes, 0, dataBytes.Length);
                fstream.Close();
            });

            string zipArchivePath = @$"{_restorePointFullDirectory}\Objects.zip";
            _configuration.AddToConfiguration(_jobConfiguration, restorePoint);

            ZipFile.CreateFromDirectory(objectsDirectory, zipArchivePath);
        }

        private void GetConfigurationFile(string rootDirectory, IRepository repository)
        {
            string configurationFileName = Configuration.ConfigurationFileName;
            string configurationPath = @$"{rootDirectory}\{configurationFileName}";

            if (repository.ExistsFile(configurationPath))
            {
                _configuration = new Configuration(configurationPath, repository);
                return;
            }

            _configuration = new Configuration(rootDirectory);
        }
    }
}