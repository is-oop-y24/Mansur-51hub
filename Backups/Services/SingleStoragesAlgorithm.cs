using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Backups.Services
{
    public class SingleStoragesAlgorithm : IAlgorithmForCreatingBackups
    {
        private string _restorePointDirectory;
        private string _restorePointFullDirectory;

        public void CreateRestorePoint(List<JobObjectInBackupJob> jobObjects, IRepository repository, int index, string jobDirectory)
        {
            var fileBuffer = new SystemFileBuffer();
            repository.CreateDirectory(jobDirectory);

            MakeZipObject(jobObjects, fileBuffer, index, jobDirectory);

            repository.SaveFiles(_restorePointFullDirectory, _restorePointDirectory);
            fileBuffer.Clear();
        }

        public void CreateRestorePointExtra(List<JobObjectInBackupJob> jobObjects, IRepository repository, string jobDirectory, string rootDirectory, string jobName, int pointsCount)
        {
            throw new NotSupportedException();
        }

        private void MakeZipObject(List<JobObjectInBackupJob> jobObjects, SystemFileBuffer fileBuffer, int index, string jobDirectory)
        {
            _restorePointDirectory = @$"{jobDirectory}\Restore_point_{index}";
            _restorePointFullDirectory = $@"{SystemFileBuffer.DirectoryName}\{_restorePointDirectory}";
            fileBuffer.CreateDirectory(_restorePointDirectory);

            string objectsDirectory = @$"{SystemFileBuffer.DirectoryName}\{jobDirectory}\Objects";
            fileBuffer.CreateDirectory($@"{jobDirectory}\Objects");

            jobObjects.ForEach(jobObject =>
            {
                string jobObjectName = jobObject.JobObject.GetObjectName();
                byte[] dataBytes = jobObject.JobObject.GetBytes().ToArray();
                string path = $@"{objectsDirectory}\{jobObjectName}_{index}";
                using FileStream fstream = File.Create(path, dataBytes.Length);
                fstream.Write(dataBytes, 0, dataBytes.Length);
                fstream.Close();
            });

            ZipFile.CreateFromDirectory(objectsDirectory, $@"{_restorePointFullDirectory}\Objects.zip");
        }
    }
}