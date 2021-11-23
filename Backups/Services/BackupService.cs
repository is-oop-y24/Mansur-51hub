using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Tools;

namespace Backups.Services
{
    public class BackupService : IBackupService
    {
        private readonly List<IBackupJob> _backupJobs;

        public BackupService(string rootDirectory)
        {
            _backupJobs = new List<IBackupJob>();

            RootDirectory = rootDirectory ?? throw new ArgumentNullException(nameof(rootDirectory));
        }

        public string RootDirectory { get; }

        public void CreateBackupJob(IBackupJob backupJob)
        {
            if (_backupJobs.Any(p => p.GetJobName().Equals(backupJob.GetJobName())))
            {
                throw new BackupsException($"Job with name {backupJob.GetJobName()} already exist");
            }

            _backupJobs.Add(backupJob);
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

        public List<IBackupJob> GetAllBackupJobs()
        {
            return _backupJobs;
        }

        string IBackupService.RootDirectory()
        {
            return RootDirectory;
        }
    }
}