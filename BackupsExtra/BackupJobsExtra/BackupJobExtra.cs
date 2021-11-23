using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Services;
using BackupsExtra.Loggers;
using BackupsExtra.PointCleaningAlgorithms;

namespace BackupsExtra.BackupJobsExtra
{
    public class BackupJobExtra : IBackupJob
    {
        private readonly BackupJob _backupJob;
        private readonly IPointsCleaningAlgorithm _algorithm;
        private readonly ILogger _logger;

        public BackupJobExtra(BackupJob backupJob, IPointsCleaningAlgorithm algorithm, ILogger logger)
        {
            _backupJob = backupJob;
            _algorithm = algorithm;
            _logger = logger;
        }

        public void CreateRestorePointInRepository(IAlgorithmForCreatingBackups algorithm, IRepository repository)
        {
            var storages = new List<Storage>();
            _backupJob
                .GetJobObjects()
                .ToList()
                .ForEach(jobObject => storages.Add(new Storage(jobObject.JobObject)));

            var restorePoint = new RestorePoint(DateTime.Now, storages);
            _backupJob.CreateRestorePoint(restorePoint);
            _logger.CreateNewMessage($"Job : {_backupJob.Name}\n{restorePoint.GetMessageForLogger()}");
            _algorithm.Execute();
            algorithm.CreateRestorePointExtra(_backupJob.GetJobObjects().ToList(), repository, _backupJob.JobDirectory, _backupJob.RootDirectory, _backupJob.GetJobName(), _backupJob.GetRestorePoints().Count);
        }

        public void CreateRestorePoint()
        {
            var storages = new List<Storage>();
            _backupJob
                .GetJobObjects()
                .ToList()
                .ForEach(jobObject => storages.Add(new Storage(jobObject.JobObject)));

            var restorePoint = new RestorePoint(DateTime.Now, storages);
            _backupJob.CreateRestorePoint(restorePoint);
            _logger.CreateNewMessage($"Job : {_backupJob.Name}\n{restorePoint.GetMessageForLogger()}");
            _algorithm.Execute();
        }

        public void CreateRestorePoint(RestorePoint newRestorePoint)
        {
            _logger.CreateNewMessage($"Job : {_backupJob.Name}\n{newRestorePoint.GetMessageForLogger()}");
            _backupJob.CreateRestorePoint(newRestorePoint);
        }

        public void AddObject(JobObject jobObject)
        {
            _backupJob.AddObject(jobObject);
        }

        public void DeleteObject(int jobObjectId)
        {
            _backupJob.DeleteObject(jobObjectId);
        }

        public List<RestorePoint> GetRestorePoints()
        {
            return _backupJob.GetRestorePoints();
        }

        public IReadOnlyList<JobObjectInBackupJob> GetJobObjects()
        {
            return _backupJob.GetJobObjects();
        }

        public string GetJobName()
        {
            return _backupJob.GetJobName();
        }
    }
}