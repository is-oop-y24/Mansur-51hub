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
        private int _index;
        public BackupJob(string name, string rootDirectory)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            _idGenerator = new IdGenerator();
            _jobObjects = new List<JobObjectInBackupJob>();
            _restorePoints = new List<RestorePoint>();
            JobDirectory = $@"{rootDirectory}\{name}";
            RootDirectory = rootDirectory;
            _index = 0;
        }

        public string Name { get; }
        public string JobDirectory { get; }
        public string RootDirectory { get; }

        public void CreateRestorePointInRepository(IAlgorithmForCreatingBackups algorithm, IRepository repository)
        {
            CreateRestorePoint();
            algorithm.CreateRestorePoint(_jobObjects, repository, _index, JobDirectory);
        }

        public void CreateRestorePoint()
        {
            if (_index < _restorePoints.Count)
            {
                _index = _restorePoints.Count;
            }
            else
            {
                _index++;
            }

            var newRestorePoint = new RestorePoint(DateTime.Now);
            _jobObjects.ForEach(jobObject =>
            {
                newRestorePoint.AddStorage(new Storage(jobObject.JobObject));
            });

            _restorePoints.Add(newRestorePoint);
        }

        public void CreateRestorePoint(RestorePoint newRestorePoint)
        {
            _restorePoints.Add(newRestorePoint);
        }

        public void AddObject(JobObject jobObject)
        {
            _jobObjects.Add(new JobObjectInBackupJob(_idGenerator.GenerateId(), jobObject));
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

        public List<RestorePoint> GetRestorePoints()
        {
            return _restorePoints;
        }

        public IReadOnlyList<JobObjectInBackupJob> GetJobObjects()
        {
            return _jobObjects;
        }

        public string GetJobName()
        {
            return Name;
        }
    }
}