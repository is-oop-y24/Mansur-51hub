using Backups.Services;

namespace Backups
{
    internal class Program
    {
        private static void Test1()
        {
            IRepository repository = new FileSystemRepository(@"C:\Users\Mansur51\Desktop\checking");
            IBackupService backupService = new BackupService("wow");
            backupService.CreateBackupJob(new BackupJob("job1", "wow"));
            IBackupJob backupJob = backupService.GetBackupJob("job1");
            backupJob.AddObject(new JobObject(@"C:\Users\Mansur51\Desktop\check.txt", "jobObject_1"));
            backupJob.AddObject(new JobObject(@"C:\Users\Mansur51\Desktop\Obektno-orientirovanny_podkhod_2020_Vaysfeld.pdf", "jobObject_2"));
            backupJob.CreateRestorePointInRepository(new SingleStoragesAlgorithm(), repository);
            int deletingObjectId = 2;
            backupJob.DeleteObject(deletingObjectId);
            backupJob.CreateRestorePointInRepository(new SplitStoragesAlgorithm(), repository);
        }

        private static void Test2()
        {
            IRepository repository = new FileSystemRepository(@"C:\Users\Mansur51\Desktop\checking");
            string rootDirectory = "wow";
            IBackupService backupService = new BackupService(rootDirectory);
            backupService.CreateBackupJob(new BackupJob("job1", rootDirectory));
            IBackupJob backupJob = backupService.GetBackupJob("job2");
            backupJob.AddObject(new JobObject(@"C:\Users\Mansur51\Desktop\check.txt", "jobObject_1"));
            for (int i = 0; i < 20; i++)
            {
                backupJob.AddObject(new JobObject(
                    @"C:\Users\Mansur51\Desktop\Obektno-orientirovanny_podkhod_2020_Vaysfeld.pdf", "jobObject_2"));
                backupJob.CreateRestorePointInRepository(new SingleStoragesAlgorithm(), repository);
                int deletingObjectId = 2;
                backupJob.DeleteObject(deletingObjectId + i);
                backupJob.CreateRestorePointInRepository(new SplitStoragesAlgorithm(), repository);
            }
        }

        private static void Main()
        {
            Test1();
            Test2();
        }
    }
}
