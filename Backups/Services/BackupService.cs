using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Tools;

namespace Backups.Services
{
    public class BackupService : IBackupService
    {
        private readonly List<IBackupJob> _backupJobs;
        private readonly IRepository _repository;
        private readonly string _rootDirectory;

        public BackupService(IRepository repository)
        {
            _backupJobs = new List<IBackupJob>();
            _repository = repository;

            _rootDirectory = $"{DateTime.Now:yy-MM-dd}";
            _repository.CreateDirectory(@$"{_rootDirectory}");
        }

        public void CreateBackupJob(string jobName)
        {
            if (_backupJobs.Any(p => p.GetJobName().Equals(jobName)))
            {
                throw new BackupsException($"Job with name {jobName} already exist");
            }

            _backupJobs.Add(new BackupJob(jobName, _repository, _rootDirectory));
        }

        public IBackupJob GetBackupJob(string jobName)
        {
            IBackupJob requiredBackupJob = _backupJobs.FirstOrDefault(p => p.GetJobName().Equals(jobName));
            if (requiredBackupJob == null)
            {
                throw new BackupsException($"Job with name {jobName} does not exist");
            }

            return requiredBackupJob;
        }
    }
}