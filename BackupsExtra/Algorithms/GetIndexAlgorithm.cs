using Backups.Services;

namespace BackupsExtra.Algorithms
{
    public class GetIndexAlgorithm
    {
        public int GetIndex(IRepository repository, string jobDirectory)
        {
            int index = 1;
            while (true)
            {
                string directoryPath = @$"{jobDirectory}\Restore_point_{index}";
                if (!repository.ExistsDirectory(directoryPath))
                {
                    return index;
                }

                index++;
            }
        }
    }
}