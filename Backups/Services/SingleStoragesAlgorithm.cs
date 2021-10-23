﻿using System;
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

        public RestorePoint CreateRestorePoint(List<JobObjectInBackupJob> jobObjects, IRepository repository, int index, string jobDirectory)
        {
            var fileBuffer = new SystemFileBuffer();
            var restorePoint = new RestorePoint(DateTime.Now);
            jobObjects.ForEach(jobObject =>
            {
                restorePoint.AddStorage(new Storage(jobObject.JobObject));
            });

            MakeZipObject(jobObjects, fileBuffer, index, jobDirectory);

            repository.SaveFiles(_restorePointFullDirectory, _restorePointDirectory);
            fileBuffer.Clear();

            return restorePoint;
        }

        private void MakeZipObject(List<JobObjectInBackupJob> jobObjects, SystemFileBuffer fileBuffer, int index, string jobDirectory)
        {
            _restorePointDirectory = @$"{jobDirectory}\Restore_point_{index}";
            _restorePointFullDirectory = $@"{fileBuffer.DirectoryName}\{_restorePointDirectory}";
            fileBuffer.CreateDirectory(_restorePointDirectory);

            string objectsDirectory = @$"{fileBuffer.DirectoryName}\{jobDirectory}\Objects";
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