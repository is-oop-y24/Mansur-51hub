namespace Backups.Services
{
    public interface IRepository
    {
        void SaveFiles(string directoryPathFrom, string directoryPathTo);
        void CreateDirectory(string path);
        byte[] GetBytes(string path);
        void CreateFile(byte[] bytes, string path);
        string GetPath();
        bool ExistsFile(string path);
        bool ExistsDirectory(string path);
    }
}