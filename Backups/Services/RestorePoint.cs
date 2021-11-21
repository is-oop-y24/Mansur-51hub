using System;
using System.Collections.Generic;
using System.Linq;

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

        public RestorePoint(DateTime date, List<Storage> jobObjectStorages)
        {
            Date = date;
            _jobObjectStorages = jobObjectStorages;
        }

        public DateTime Date { get; }

        public void AddStorage(Storage storage)
        {
            _jobObjectStorages.Add(storage);
        }

        public List<Storage> GetJobObjectStorages()
        {
            return _jobObjectStorages;
        }

        public string GetMessageForLogger()
        {
            string message = "Restore point created\nObjects:\n";
            var objectsMessage = _jobObjectStorages
                .Select(objects => objects.GetMessageForLogger())
                .ToList();
            string objectsTotalMessage = string.Join(Environment.NewLine, objectsMessage);
            return message + objectsTotalMessage;
        }
    }
}