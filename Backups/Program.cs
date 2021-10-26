using Backups.Services;

namespace Backups
{
    internal class Program
    {
        private static void Test1()
        {
            IRepository repository = new FileSystemRepository(@"C:\Users\Mansur51\Desktop\checking");
            IBackupService backupService = new BackupService();
            backupService.CreateBackupJob("job1");
            IBackupJob backupJob = backupService.GetBackupJob("job1");
            backupJob.AddObject(new JobObject(@"C:\Users\Mansur51\Desktop\check.txt", "jobObject_1"));
            backupJob.AddObject(new JobObject(@"C:\Users\Mansur51\Desktop\Obektno-orientirovanny_podkhod_2020_Vaysfeld.pdf", "jobObject_2"));
            backupJob.CreateRestorePoint(new SingleStoragesAlgorithm(), repository);
            int deletingObjectId = 2;
            backupJob.DeleteObject(deletingObjectId);
            backupJob.CreateRestorePoint(new SplitStoragesAlgorithm(), repository);
        }

        private static void Test2()
        {
            IRepository repository = new FileSystemRepository(@"C:\Users\Mansur51\Desktop\checking");
            IBackupService backupService = new BackupService();
            backupService.CreateBackupJob("job2");
            IBackupJob backupJob = backupService.GetBackupJob("job2");
            backupJob.AddObject(new JobObject(@"C:\Users\Mansur51\Desktop\check.txt", "jobObject_1"));
            for (int i = 0; i < 20; i++)
            {
                backupJob.AddObject(new JobObject(
                    @"C:\Users\Mansur51\Desktop\Obektno-orientirovanny_podkhod_2020_Vaysfeld.pdf", "jobObject_2"));
                backupJob.CreateRestorePoint(new SingleStoragesAlgorithm(), repository);
                int deletingObjectId = 2;
                backupJob.DeleteObject(deletingObjectId + i);
                backupJob.CreateRestorePoint(new SplitStoragesAlgorithm(), repository);
            }
        }

        private static void Main()
        {
            Test1();
            Test2();
        }
    }
}
