using System;
using System.IO;
using System.Linq;
using Backups.Services;

namespace BackupsExtra.Restorings
{
    public class ToDifferentLocationRestoreAlgorithm : IRestorePointAlgorithm
    {
        private readonly RestorePoint _restorePoint;
        private readonly string _directoryPath;

        public ToDifferentLocationRestoreAlgorithm(RestorePoint restorePoint, string directoryPath)
        {
            _restorePoint = restorePoint;
            _directoryPath = directoryPath;
        }

        public void Execute()
        {
            var directoryInfo = new DirectoryInfo(@$"{_directoryPath}\RestoredPoint_{DateTime.Now:d}");
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            _restorePoint.GetJobObjectStorages().ForEach(storage =>
            {
                string jobObjectName = storage.JobObject.GetObjectName();
                byte[] dataBytes = storage.JobObject.GetBytes().ToArray();
                using FileStream fstream = File.Create(@$"{_directoryPath}\RestoredPoint_{DateTime.Now:d}\{jobObjectName}");
                fstream.Write(dataBytes, 0, dataBytes.Length);
                fstream.Close();
            });
        }
    }
}