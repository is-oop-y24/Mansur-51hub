using System.IO;

namespace Backups.Services
{
    public class SystemFileBuffer
    {
        public SystemFileBuffer()
        {
            var directoryInfo = new DirectoryInfo(DirectoryName);
            if (!directoryInfo.Exists)
            {
                Directory.CreateDirectory(DirectoryName);
            }
        }

        public static string DirectoryName { get; } = @"C:\backups";

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