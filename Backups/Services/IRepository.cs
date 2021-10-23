namespace Backups.Services
{
    public interface IRepository
    {
        void SaveFiles(string directoryPathFrom, string directoryPathTo);
        void CreateDirectory(string path);
    }
}