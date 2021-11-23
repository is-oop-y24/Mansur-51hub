using System.IO;
using System.Linq;
using Backups.Services;

namespace BackupsExtra.Restorings
{
    public class ToOriginalLocationRestoreAlgorithm : IRestorePointAlgorithm
    {
        private readonly RestorePoint _restorePoint;

        public ToOriginalLocationRestoreAlgorithm(RestorePoint restorePoint)
        {
            _restorePoint = restorePoint;
        }

        public void Execute()
        {
            _restorePoint.GetJobObjectStorages().ForEach(storage =>
            {
                var fileInfo = new FileInfo(@$"{storage.JobObject.OriginalPath}");
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }

                string jobObjectName = storage.JobObject.GetObjectName();
                byte[] dataBytes = storage.JobObject.GetBytes().ToArray();
                using FileStream fstream = File.Create(@$"{storage.JobObject.OriginalPath}");
                fstream.Write(dataBytes, 0, dataBytes.Length);
                fstream.Close();
            });
        }
    }
}