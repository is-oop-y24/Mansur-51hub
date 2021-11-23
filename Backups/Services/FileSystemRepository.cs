using System.IO;
using Backups.Tools;

namespace Backups.Services
{
    public class FileSystemRepository : IRepository
    {
        private readonly string _path;

        public FileSystemRepository(string path)
        {
            var directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists)
            {
                Directory.CreateDirectory(path);
            }

            _path = path;
        }

        public void CreateDirectory(string path)
        {
            var directoryInfo = new DirectoryInfo(@$"{_path}\{path}");
            if (!directoryInfo.Exists)
            {
                Directory.CreateDirectory(@$"{_path}\{path}");
            }
        }

        public byte[] GetBytes(string path)
        {
            var fileInfo = new FileInfo(@$"{_path}\{path}");
            if (!fileInfo.Exists)
            {
                throw new BackupsException("File does not exist");
            }

            using FileStream fstream = File.OpenRead(@$"{_path}\{path}");
            byte[] bytes = new byte[fstream.Length];
            fstream.Read(bytes, 0, bytes.Length);
            fstream.Close();
            return bytes;
        }

        public void CreateFile(byte[] bytes, string path)
        {
            var fileInfo = new FileInfo(@$"{_path}\{path}");
            if (!fileInfo.Exists)
            {
                fileInfo.Delete();
            }

            using FileStream fstream = File.Create(@$"{_path}\{path}");
            fstream.Write(bytes, 0, bytes.Length);
            fstream.Close();
        }

        public string GetPath()
        {
            return _path;
        }

        public bool ExistsFile(string path)
        {
            var fileInfo = new FileInfo(@$"{_path}\{path}");
            return fileInfo.Exists;
        }

        public bool ExistsDirectory(string path)
        {
            var directoryInfo = new DirectoryInfo(@$"{_path}\{path}");
            return directoryInfo.Exists;
        }

        public void SaveFiles(string directoryPathFrom, string directoryPathTo)
        {
            var directoryInfo = new DirectoryInfo(directoryPathFrom);

            if (!directoryInfo.Exists)
            {
                throw new BackupsException($"Directory {directoryPathFrom} does not exist");
            }

            Directory.Move(directoryPathFrom, $@"{_path}\{directoryPathTo}");
        }
    }
}