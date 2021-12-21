using Reports.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reports.Server.Services
{
    public interface ITaskLoggerService
    {
        List<string> GetTaskLogs(Guid id);
        
        Task<TaskLoggerPage> AddLog(Guid id, string logMessage);
    }
}
