using System;
using System.Collections.Generic;

namespace BackupsExtra.Configurations
{
    public class RestorePointConfiguration
    {
        private readonly List<JobObjectConfiguration> _jobObjects;

        public RestorePointConfiguration(DateTime creationTime)
        {
            _jobObjects = new List<JobObjectConfiguration>();
            CreationTime = creationTime;
        }

        public DateTime CreationTime { get; }

        public void AddJobObject(JobObjectConfiguration jobObject)
        {
            _jobObjects.Add(jobObject);
        }

        public IReadOnlyList<JobObjectConfiguration> GetJobObjects()
        {
            return _jobObjects;
        }
    }
}