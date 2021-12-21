using Microsoft.EntityFrameworkCore;
using Reports.DAL.Entities;
using Reports.Server.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Server.Services
{
    public class TaskService : ITaskService
    {
        private readonly ReportsDatabaseContext _context;
        private readonly ITaskLoggerService _taskLoggerService;

        public TaskService(ReportsDatabaseContext context, ITaskLoggerService taskLoggerService)
        {
            _context = context;
            _taskLoggerService = taskLoggerService;
        }

        public async Task<Problem> Create(Guid responsibleEmployeeId, string description)
        {
            Employee responsibleEmployee = _context.Employees.FirstOrDefault(p => p.Id.Equals(responsibleEmployeeId));
            if (responsibleEmployee != null) {
                var task = new Problem(Guid.NewGuid(), Status.Open)
                {
                    ResponsibleEmployeeId = responsibleEmployee.Id,
                    Description = Encoding.ASCII.GetBytes(description)
                };

                _context.Problems.Add(task);
                _context.EmployeeProblems.Find(responsibleEmployeeId).Problems.Add(task);
                _context.TaskLogs.Add(new TaskLoggerPage(task.Id));
                _context.Reports.Add(new Report(task.Id));
                await _context.SaveChangesAsync();
                await _taskLoggerService.AddLog(task.Id, $"[{DateTime.Now:G}]: Responsible employee id: {responsibleEmployeeId}");
                return task;
            }

            return null;
        }

        public Problem FindById(Guid id)
        {
            Problem problemToFind = _context.Problems.FirstOrDefault(p => p.Id.Equals(id));
            return problemToFind;
        }

        public string FindDescription(Guid id)
        {
            Problem task = _context.Problems.FirstOrDefault(t => t.Id.Equals(id));
            if (task != null)
            {
                return Encoding.ASCII.GetString(task.Description);
            }

            return null;
        }

        public List<Problem> FindEmployeeTasks(Guid employeeId)
        {
            EmployeeTasks tasks = _context.EmployeeProblems
                .Include(p => p.Problems)
                .FirstOrDefault(e => e.Id.Equals(employeeId));

            if(tasks != null)
            {
                return tasks.Problems.ToList();
            }

            return null;
        }

        public List<Problem> FindSubordinatesTaks(Guid id)
        {
            Employee superVisor = _context.Employees.FirstOrDefault(s => s.Id.Equals(id));
            if (superVisor != null)
            {
                var result = new List<Problem>();
                _context
                    .Subordinates
                    .Include(s => s.Subordinates)
                    .FirstOrDefault(s => s.Id.Equals(superVisor.Id))
                    .Subordinates
                    .ToList()
                    .ForEach(employee =>
                    {
                        var employeeProblems = _context.EmployeeProblems
                        .Include(p => p.Problems)
                        .First(e => e.Id.Equals(employee.Id))
                        .Problems
                        .ToList();
                        result.AddRange(employeeProblems);
                    }
                    );

                return result;
            }

            return null;
        }

        public List<Problem> GetTasks()
        {
            return _context.Problems.ToList();
        }

        public async Task<Problem> UpdateDescription(Guid id, string newDescription)
        {
            Problem task = _context.Problems.FirstOrDefault(t => t.Id.Equals(id));

            if(task != null)
            {
                task.Description = Encoding.ASCII.GetBytes(newDescription);
                await _context.SaveChangesAsync();
                await _taskLoggerService.AddLog(task.Id, $"[{DateTime.Now:G}]: Description updated");
                return task;
            }

            return null;
        }

        public async Task<Problem> UpdateResponsibleEmployee(Guid id, Guid newEmployeeId)
        {
            Problem task = _context.Problems.FirstOrDefault(p => p.Id.Equals(id));
            Employee previousEmployee = _context.Employees.FirstOrDefault(e => e.Id.Equals(task.ResponsibleEmployeeId));
            Employee newEmployee = _context.Employees.FirstOrDefault(e => e.Id.Equals(newEmployeeId));

            if (task != null && previousEmployee != null && newEmployee != null)
            {
                _context.EmployeeProblems.First(p => p.Id.Equals(previousEmployee.Id)).Problems.Remove(task);
                _context.EmployeeProblems.First(p => p.Id.Equals(newEmployeeId)).Problems.Add(task);
                task.ResponsibleEmployeeId = newEmployeeId;
                await _context.SaveChangesAsync();
                await _taskLoggerService.AddLog(task.Id, $"[{DateTime.Now:G}]: New responsible employee id: {newEmployeeId}");
                return task;
            }

            return null;
        }

        public async Task<Problem> UpdateStatus(Guid id, Status newStatus)
        {
            Problem task = _context.Problems.FirstOrDefault(p => p.Id.Equals(id));

            if (task != null)
            {
                task.Status = newStatus;
                await _context.SaveChangesAsync();
               await _taskLoggerService.AddLog(task.Id, $"[{DateTime.Now:G}]: New status: {Enum.GetName(typeof(Status), newStatus)}");
                return task;
            }

            return null;
        }
    }
}
