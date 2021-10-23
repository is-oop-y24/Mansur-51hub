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
        public RestorePoint CreateRestorePoint(List<JobObjectInBackupJob> jobObjects, IRepository repository, int index, string jobDirectory)
        {
            var fileBuffer = new SystemFileBuffer();
            var restorePoint = new RestorePoint(DateTime.Now);

            jobObjects.ForEach(jobObject =>
            {
                var storage = new Storage(jobObject.JobObject);
                restorePoint.AddStorage(storage);
                MakeZipObject(jobObject, fileBuffer, index, jobDirectory);
            });

            repository.SaveFiles(_restorePointFullDirectory, _restorePointDirectory);
            fileBuffer.Clear();

            return restorePoint;
        }

        private void MakeZipObject(JobObjectInBackupJob jobObject, SystemFileBuffer fileBuffer, int index, string jobDirectory)
        {
            byte[] dataBytes = jobObject.JobObject.GetBytes().ToArray();

            _restorePointDirectory = @$"{jobDirectory}\Restore_point_{index}";
            _restorePointFullDirectory = $@"{fileBuffer.DirectoryName}\{_restorePointDirectory}";
            fileBuffer.CreateDirectory(_restorePointDirectory);

            string jobObjectName = jobObject.JobObject.GetObjectName();
            string objectFullDirectoryForZip = $@"{fileBuffer.DirectoryName}\{_restorePointDirectory}\{jobObjectName}_{index}";

            string objectFullDirectoryForCopy = @$"{fileBuffer.DirectoryName}\{jobDirectory}\{jobObjectName}_{index}";
            fileBuffer.CreateDirectory($@"{jobDirectory}\{jobObjectName}_{index}");

            string path = $@"{objectFullDirectoryForCopy}\{jobObjectName}_{index}";
            using FileStream fstream = File.Create(path, dataBytes.Length);

            fstream.Write(dataBytes, 0, dataBytes.Length);
            fstream.Close();

            ZipFile.CreateFromDirectory(objectFullDirectoryForCopy, $"{objectFullDirectoryForZip}.zip");
        }
    }
}