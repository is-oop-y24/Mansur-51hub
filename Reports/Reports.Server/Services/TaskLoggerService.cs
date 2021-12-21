using Microsoft.EntityFrameworkCore;
using Reports.DAL.Entities;
using Reports.Server.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reports.Server.Services
{
    public class TaskLoggerService : ITaskLoggerService
    {
        private readonly ReportsDatabaseContext _context;

        public TaskLoggerService(ReportsDatabaseContext context)
        {
            _context = context;
        }

        public async Task<TaskLoggerPage> AddLog(Guid id, string logMessage)
        {
            var messages = _context.TaskLogs.Include(m => m.Messages).FirstOrDefault(t => t.Id.Equals(id)).Messages.ToList();
             if(messages != null)
            {
                messages.Add(new Message(logMessage));
                TaskLoggerPage page = _context.TaskLogs.First(t => t.Id.Equals(id));
                page.Messages = messages;
                _context.TaskLogs.Update(page);
                return page;
            }

            return null;
        }

        public List<string> GetTaskLogs(Guid id)
        {
            var logs = _context.TaskLogs
                 .FirstOrDefault(t => t.Id.Equals(id)).Messages.Select(m => m.Information).ToList();

            return logs;
        }
    }
}
