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