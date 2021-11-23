using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Linq;
using Backups.Services;
using BackupsExtra.PointCleaningAlgorithms;
using BackupsExtra.Tools;

namespace BackupsExtra.Configurations
{
    public class RestoreFromConfiguration
    {
        private readonly IBackupService _backupService;
        private readonly IRepository _repository;
        private readonly string _rootDirectory;

        public RestoreFromConfiguration(IBackupService backupService, IRepository repository, string rootDirectory)
        {
            _backupService = backupService;
            _repository = repository;
            _rootDirectory = rootDirectory ?? throw new ArgumentNullException(nameof(rootDirectory));
        }

        public void Execute()
        {
            string configurationPath = $@"{_rootDirectory}\{Configuration.ConfigurationFileName}";
            if (!_repository.ExistsFile(configurationPath))
            {
                throw new BackupsExtraException("Could not find configuration file");
            }

            byte[] bytes = _repository.GetBytes(configurationPath);
            string path = @$"{SystemFileBuffer.DirectoryName}\config.xml";
            var buffer = new SystemFileBuffer();
            CreateFileInBuffer(bytes, path);

            var configuration = XDocument.Load(path);

            configuration.Root?.Elements("Job").ToList().ForEach(job =>
            {
                string jobName = job.FirstAttribute?.Value;
                int pointsCount = int.Parse(job.LastAttribute.Value);
                _backupService.CreateBackupJob(new BackupJob(jobName, _backupService.RootDirectory()));
                IBackupJob jobInService = _backupService.GetBackupJob(jobName);

                job.Elements("RestorePoint").ToList().ForEach(restorePoint =>
                {
                    string[] time = restorePoint.FirstAttribute?.Value.Split('-');
                    var restorePointInService = new RestorePoint(new DateTime(int.Parse(time[0]), int.Parse(time[1]), int.Parse(time[2])));

                    restorePoint.Elements("JobObject").ToList().ForEach(jobObject =>
                    {
                        var jobObjectConfiguration = new JobObjectConfiguration(jobObject);

                        bool zipExist = ExistZip(jobObjectConfiguration.ZipPath);
                        if (!zipExist)
                        {
                            ZipFile.ExtractToDirectory(@$"{_repository.GetPath()}\{jobObjectConfiguration.ZipPath}", $@"{SystemFileBuffer.DirectoryName}\{jobObjectConfiguration.ZipPath}", true);
                        }

                        var jobObjectInService = new JobObject($@"{SystemFileBuffer.DirectoryName}\{jobObjectConfiguration.Path}", jobObjectConfiguration.Name);
                        jobObjectInService.ChangeOriginalPath(jobObjectConfiguration.OriginalPath);
                        restorePointInService.AddStorage(new Storage(jobObjectInService));
                    });

                    jobInService.CreateRestorePoint(restorePointInService);
                });
                new ByTheNumberOfPointsAlgorithm(jobInService, pointsCount).Execute();

                AddCurrentObjectsToJob(job.Elements("RestorePoint").ToList().Last(), jobInService);
            });

            buffer.Clear();
        }

        public void CreateFileInBuffer(byte[] bytes, string path)
        {
            using FileStream fstream = File.Create(path, bytes.Length);
            fstream.Write(bytes, 0, bytes.Length);
            fstream.Close();
        }

        private bool ExistZip(string path)
        {
            var directoryInfo = new DirectoryInfo(@$"SystemFileBuffer.DirectoryName\{path}");
            return directoryInfo.Exists;
        }

        private void AddCurrentObjectsToJob(XElement restorePoint, IBackupJob backupJob)
        {
            restorePoint.Elements("JobObject").ToList().ForEach(jobObject =>
            {
                var jobObjectConfiguration = new JobObjectConfiguration(jobObject);

                bool zipExist = ExistZip(jobObjectConfiguration.ZipPath);
                if (!zipExist)
                {
                    ZipFile.ExtractToDirectory(@$"{_repository.GetPath()}\{jobObjectConfiguration.ZipPath}", $@"{SystemFileBuffer.DirectoryName}\{jobObjectConfiguration.ZipPath}", true);
                }

                var jobObjectInService = new JobObject($@"{SystemFileBuffer.DirectoryName}\{jobObjectConfiguration.Path}", jobObjectConfiguration.Name);
                backupJob.AddObject(jobObjectInService);
            });
        }
    }
}