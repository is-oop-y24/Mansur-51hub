using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Backups.Services;
using BackupsExtra.Configurations;

namespace BackupsExtra.Algorithms
{
    public class SplitStoragesAlgorithmExtra : IAlgorithmForCreatingBackups
    {
        private string _restorePointDirectory;
        private string _restorePointFullDirectory;
        private Configuration _configuration;
        private JobConfiguration _jobConfiguration;

        public void CreateRestorePoint(List<JobObjectInBackupJob> jobObjects, IRepository repository, int index, string jobDirectory)
        {
            new SplitStoragesAlgorithm().CreateRestorePoint(jobObjects, repository, index, jobDirectory);
        }

        public void CreateRestorePointExtra(List<JobObjectInBackupJob> jobObjects, IRepository repository, string jobDirectory, string rootDirectory, string jobName, int pointsCount)
        {
            var fileBuffer = new SystemFileBuffer();
            repository.CreateDirectory(jobDirectory);
            fileBuffer.CreateDirectory(jobDirectory);
            _jobConfiguration = new JobConfiguration(jobName, rootDirectory, pointsCount);
            GetConfigurationFile(rootDirectory, repository);
            int index = new GetIndexAlgorithm().GetIndex(repository, jobDirectory);

            var restorePoint = new RestorePointConfiguration(DateTime.Now);
            jobObjects.ForEach(jobObject =>
            {
                MakeZipObject(jobObject, fileBuffer, index, jobDirectory, restorePoint);
            });

            repository.SaveFiles(_restorePointFullDirectory, _restorePointDirectory);
            _configuration.AddToConfiguration(_jobConfiguration, restorePoint);

            string configurationDirectoryInRepository = @$"{rootDirectory}\{Configuration.ConfigurationFileName}";
            repository.CreateFile(_configuration.GetBytesData(rootDirectory), configurationDirectoryInRepository);
            fileBuffer.Clear();
        }

        private void MakeZipObject(JobObjectInBackupJob jobObject, SystemFileBuffer fileBuffer, int index, string jobDirectory, RestorePointConfiguration restorePoint)
        {
            byte[] dataBytes = jobObject.JobObject.GetBytes().ToArray();

            _restorePointDirectory = @$"{jobDirectory}\Restore_point_{index}";
            _restorePointFullDirectory = $@"{SystemFileBuffer.DirectoryName}\{_restorePointDirectory}";
            fileBuffer.CreateDirectory(_restorePointDirectory);

            string jobObjectName = jobObject.JobObject.GetObjectName();
            string objectFullDirectoryForZip = $@"{SystemFileBuffer.DirectoryName}\{_restorePointDirectory}\{jobObjectName}_{index}";

            string objectFullDirectoryForCopy = @$"{SystemFileBuffer.DirectoryName}\{jobDirectory}\{jobObjectName}_{index}";
            fileBuffer.CreateDirectory($@"{jobDirectory}\{jobObjectName}_{index}");

            string path = $@"{objectFullDirectoryForCopy}\{jobObjectName}_{index}";
            using FileStream fstream = File.Create(path, dataBytes.Length);

            fstream.Write(dataBytes, 0, dataBytes.Length);
            fstream.Close();

            var jobObjectConfiguration = new JobObjectConfiguration(jobObjectName, $@"{_restorePointDirectory}\{jobObjectName}_{index}.zip\{jobObjectName}_{index}", @$"{_restorePointDirectory}\{jobObjectName}_{index}.zip", jobObject.JobObject.OriginalPath);
            restorePoint.AddJobObject(jobObjectConfiguration);

            ZipFile.CreateFromDirectory(objectFullDirectoryForCopy, $"{objectFullDirectoryForZip}.zip");
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