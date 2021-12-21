using Reports.DAL.Entities;
using Reports.Server.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Server.Services
{
    public class ReportService : IReportService
    {
        private readonly ReportsDatabaseContext _context;

        public ReportService(ReportsDatabaseContext context)
        {
            _context = context;
        }

        public async Task<WeeklyReport> AddReport(Guid id, Report report)
        {
            WeeklyReport weeklyReport = _context.WeeklyReports.FirstOrDefault(r => r.Id.Equals(id));
            if(weeklyReport != null)
            {
                weeklyReport.Reports.Add(report);
                await _context.SaveChangesAsync();
                return weeklyReport;
            }

            return null;

        }

        public async Task<WeeklyReport> CreateWeeklyReport()
        {
            var report = new WeeklyReport();
            _context.WeeklyReports.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }

        public List<Problem> GetTasks()
        {
            const int dayDifference = 7;
            var result = _context.Problems.Where(p => (DateTime.Now - p.CreationTime).TotalDays > dayDifference).ToList();

            return result;
        }

        public async Task<WeeklyReport> UpdateDescription(Guid id, string newDescription)
        {
            WeeklyReport weeklyReport = _context.WeeklyReports.FirstOrDefault(r => r.Id.Equals(id));
            if (weeklyReport != null)
            {
                weeklyReport.Description = Encoding.ASCII.GetBytes(newDescription);
                await _context.SaveChangesAsync();
                return weeklyReport;
            }

            return null;
        }
    }
}
