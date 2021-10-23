using System;
using System.Collections.Generic;

namespace Backups.Services
{
    public class RestorePoint
    {
        private readonly DateTime _date;
        private readonly List<Storage> _jobObjectStorages;

        public RestorePoint(DateTime date)
        {
            _date = date;
            _jobObjectStorages = new List<Storage>();
        }

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