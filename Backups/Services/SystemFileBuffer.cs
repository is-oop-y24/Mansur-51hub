using System.IO;

namespace Backups.Services
{
    public class SystemFileBuffer
    {
        public SystemFileBuffer()
        {
            const string bufferDirectoryName = @"C:\backups";
            DirectoryName = bufferDirectoryName;
            var directoryInfo = new DirectoryInfo(bufferDirectoryName);
            if (!directoryInfo.Exists)
            {
                Directory.CreateDirectory(bufferDirectoryName);
            }
        }

        public string DirectoryName { get; }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(@$"{DirectoryName}\{path}");
        }

        public void Clear()
        {
            var directoryInfo = new DirectoryInfo(DirectoryName);
            if (directoryInfo.Exists)
            {
                Directory.Delete(DirectoryName, true);
            }
        }
    }
}