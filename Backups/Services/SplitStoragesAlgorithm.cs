using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Backups.Services
{
    public class SplitStoragesAlgorithm : IAlgorithmForCreatingBackups
    {
        private string _restorePointDirectory;
        private string _restorePointFullDirectory;
        public void CreateRestorePoint(List<JobObjectInBackupJob> jobObjects, IRepository repository, int index, string jobDirectory)
        {
            var fileBuffer = new SystemFileBuffer();
            repository.CreateDirectory(jobDirectory);

            jobObjects.ForEach(jobObject =>
            {
                MakeZipObject(jobObject, fileBuffer, index, jobDirectory);
            });

            repository.SaveFiles(_restorePointFullDirectory, _restorePointDirectory);
            fileBuffer.Clear();
        }

        public void CreateRestorePointExtra(List<JobObjectInBackupJob> jobObjects, IRepository repository, string jobDirectory, string rootDirectory, string jobName, int pointsCount)
        {
            throw new NotSupportedException();
        }

        private void MakeZipObject(JobObjectInBackupJob jobObject, SystemFileBuffer fileBuffer, int index, string jobDirectory)
        {
            byte[] dataBytes = jobObject.JobObject.GetBytes().ToArray();

            _restorePointDirectory = @$"{jobDirectory}\Restore_point_{index}";
            _restorePointFullDirectory = $@"{SystemFileBuffer.DirectoryName}\{_restorePointDirectory}";
            fileBuffer.CreateDirectory(_restorePointDirectory);

            string jobObjectName = jobObject.JobObject.GetObjectName();
            string objectFullDirectoryForZip = $@"{SystemFileBuffer.DirectoryName}\{_restorePointDirectory}\{jobObjectName}_{index}";

            string objectFullDirectoryForCopy = @$"{SystemFileBuffer.DirectoryName}\{jobDirectory}\{jobObjectName}_{index}";
            fileBuffer.CreateDirectory($@"{jobDirectory}\{jobObjectName}_{index}");

            string path = $@"{objectFullDirectoryForCopy}\{jobObjectName}_{index}";
            using FileStream fstream = File.Create(path, dataBytes.Length);

            fstream.Write(dataBytes, 0, dataBytes.Length);
            fstream.Close();

            ZipFile.CreateFromDirectory(objectFullDirectoryForCopy, $"{objectFullDirectoryForZip}.zip");
        }
    }
}