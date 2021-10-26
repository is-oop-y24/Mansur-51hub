using System;
using System.Collections.Generic;
using System.IO;
using Backups.Tools;

namespace Backups.Services
{
    public class JobObject : IJobObject
    {
        private readonly byte[] _bytes;
        private readonly string _name;

        public JobObject(string path, string name)
        {
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                throw new BackupsException("File does not exist");
            }

            _name = name ?? throw new ArgumentNullException(nameof(name));
            using FileStream fstream = File.OpenRead(path);
            _bytes = new byte[fstream.Length];
            fstream.Read(_bytes, 0, _bytes.Length);
            fstream.Close();
        }

        public IReadOnlyCollection<byte> GetBytes()
        {
            return Array.AsReadOnly(_bytes);
        }

        public string GetObjectName()
        {
            return _name;
        }
    }
}