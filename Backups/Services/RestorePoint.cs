using System;
using System.Collections.Generic;

namespace Backups.Services
{
    public class RestorePoint
    {
        private readonly List<Storage> _jobObjectStorages;

        public RestorePoint(DateTime date)
        {
            Date = date;
            _jobObjectStorages = new List<Storage>();
        }

        public DateTime Date { get; }

        public void AddStorage(Storage storage)
        {
            _jobObjectStorages.Add(storage);
        }

        public IReadOnlyList<Storage> GetJobObjectStorages()
        {
            return _jobObjectStorages;
        }
    }
}