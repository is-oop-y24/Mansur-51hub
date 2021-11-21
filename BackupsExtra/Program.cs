using Backups.Services;
using BackupsExtra.Algorithms;
using BackupsExtra.BackupJobsExtra;
using BackupsExtra.Configurations;
using BackupsExtra.Loggers;
using BackupsExtra.Loggers.LoggerConfigurations;
using BackupsExtra.MergesAlgorithm;
using BackupsExtra.PointCleaningAlgorithms;
using BackupsExtra.Restorings;

namespace BackupsExtra
{
    internal class Program
    {
        private static IRepository _repository;
        private static IBackupService _checkService;
        private static IBackupService _backupService;

        public static void Setup()
        {
            _repository = new FileSystemRepository(@"C:\Users\Mansur51\Desktop\checking");
            _checkService = new BackupService("check");
            _backupService = new BackupService("beck");
        }

        public static void CreateJobs_RestoreFromConfiguration_SuccessfullyDone()
        {
            Setup();
            for (int i = 0; i < 5; i++)
            {
                var job = new BackupJob($"job{i}", _backupService.RootDirectory());
                var jobExtra = new BackupJobExtra(job, new ByTheNumberOfPointsAlgorithm(job, 2), new FileLogger(new LoggerTimeConfiguration(), @"C:\Users\Mansur51\Desktop\checking\log.txt"));
                _backupService.CreateBackupJob(jobExtra);
                IBackupJob backupJob = _backupService.GetBackupJob($"job{i}");
                backupJob.AddObject(new JobObject(@"C:\Users\Mansur51\Desktop\check.txt", "jobObject_1"));
                backupJob.AddObject(new JobObject(
                    @"C:\Users\Mansur51\Desktop\Obektno-orientirovanny_podkhod_2020_Vaysfeld.pdf", "jobObject_2"));
                for (int j = 0; j < 4; j++)
                {
                    backupJob.CreateRestorePointInRepository(new SingleStoragesAlgorithmExtra(), _repository);
                }
            }

            var restore = new RestoreFromConfiguration(_checkService, _repository, "beck");
            restore.Execute();
        }

        private static void MergeRestorePoints_SuccessfullyDone()
        {
            CreateJobs_RestoreFromConfiguration_SuccessfullyDone();
            RestorePoint point1 = _checkService.GetBackupJob("job0").GetRestorePoints()[0];
            RestorePoint point2 = _checkService.GetBackupJob("job0").GetRestorePoints()[1];
            new MergeAlgorithm(_checkService.GetBackupJob("job0"), point1, point2).Execute();
        }

        private static void RestoreToOriginalLocation_SuccessfullyDone()
        {
            CreateJobs_RestoreFromConfiguration_SuccessfullyDone();
            new ToOriginalLocationRestoreAlgorithm(_checkService.GetBackupJob("job2").GetRestorePoints()[1]).Execute();
        }

        private static void RestoreToDifferentLocation_SuccessfullyDone()
        {
            CreateJobs_RestoreFromConfiguration_SuccessfullyDone();
            new ToDifferentLocationRestoreAlgorithm(_checkService.GetBackupJob("job3").GetRestorePoints()[0], @"C:\Users\Mansur51\Desktop").Execute();
        }

        private static void Main()
        {
            RestoreToDifferentLocation_SuccessfullyDone();
        }
    }
}