using Reports.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reports.Server.Services
{
    public interface IReportService
    {
        List<Problem> GetTasks();
        Task<WeeklyReport> CreateWeeklyReport();
        Task<WeeklyReport> AddReport(Guid id, Report report);
        Task<WeeklyReport> UpdateDescription(Guid id, string newDescription);
    }
}
