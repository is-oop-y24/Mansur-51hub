using Microsoft.EntityFrameworkCore;
using Reports.DAL.Entities;

namespace Reports.Server.Database
{
    public class ReportsDatabaseContext : DbContext
    {
        public ReportsDatabaseContext(DbContextOptions<ReportsDatabaseContext> options) : base(options)
        {
        //    Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<Stuff> Subordinates { get; set; }
        public DbSet<EmployeeTasks> EmployeeProblems { get; set; }
        public DbSet<TaskLoggerPage> TaskLogs { get; set; }
        public DbSet<Report> Reports { get; set; } 
        public DbSet<WeeklyReport> WeeklyReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<Problem>().ToTable("Problems");
            modelBuilder.Entity<Stuff>().ToTable("Subordinates");
            modelBuilder.Entity<EmployeeTasks>().ToTable("EmployeeTasks");
            modelBuilder.Entity<TaskLoggerPage>().ToTable("TasksLogs");
            modelBuilder.Entity<Report>().ToTable("Reports");
            modelBuilder.Entity<WeeklyReport>().ToTable("WeeklyReports");

            base.OnModelCreating(modelBuilder);
        }
    }
}