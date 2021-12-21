using Reports.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reports.Server.Services
{
    public interface ITaskService
    {
        Problem FindById(Guid id);
        
        Task<Problem> Create(Guid responsibleEmployeeId, string description);
        
        List<Problem> FindEmployeeTasks(Guid employeeId);
        
        string FindDescription(Guid id);
        
        List<Problem> GetTasks();

        Task<Problem> UpdateDescription(Guid id, string newDescription);

        List<Problem> FindSubordinatesTaks(Guid id);

        Task<Problem> UpdateStatus(Guid id, Status newStatus);

        Task<Problem> UpdateResponsibleEmployee(Guid id, Guid newEmployeeId);
    }
}
