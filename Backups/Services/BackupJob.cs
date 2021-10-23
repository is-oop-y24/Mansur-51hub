using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Tools;

namespace Backups.Services
{
    public class BackupJob : IBackupJob
    {
        private readonly IdGenerator _idGenerator;
        private readonly List<JobObjectInBackupJob> _jobObjects;
        private readonly List<RestorePoint> _restorePoints;
        private readonly string _name;
        private readonly string _jobDirectory;
        private int _index;
        public BackupJob(string name, string rootDirectory)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _idGenerator = new IdGenerator();
            _jobObjects = new List<JobObjectInBackupJob>();
            _restorePoints = new List<RestorePoint>();
            _jobDirectory = $@"{rootDirectory}\{name}";
            _index = 0;
        }

        public void AddObject(IJobObject jobObject)
        {
            _jobObjects.Add(new JobObjectInBackupJob(_idGenerator.GenerateId(), jobObject));
        }

        public void CreateRestorePoint(IAlgorithmForCreatingBackups algorithm, IRepository repository)
        {
            _index++;
            var newRestorePoint = new RestorePoint(DateTime.Now);
            _jobObjects.ForEach(jobObject =>
            {
                newRestorePoint.AddStorage(new Storage(jobObject.JobObject));
            });

            _restorePoints.Add(newRestorePoint);
            algorithm.CreateRestorePoint(_jobObjects, repository, _index, _jobDirectory);
        }

        public void DeleteObject(int jobObjectId)
        {
            JobObjectInBackupJob requiredObject = _jobObjects.FirstOrDefault(p => p.Id.Equals(jobObjectId));
            if (requiredObject == null)
            {
                throw new BackupsException($"There is no object with id {jobObjectId} in backup job");
            }

            _jobObjects.Remove(requiredObject);
        }

        public IReadOnlyList<RestorePoint> GetRestorePoints()
        {
            return _restorePoints;
        }

        public IReadOnlyList<JobObjectInBackupJob> GetJobObjects()
        {
            return _jobObjects;
        }

        public string GetJobName()
        {
            return _name;
        }
    }
}