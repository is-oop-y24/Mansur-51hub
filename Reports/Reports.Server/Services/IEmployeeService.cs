using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reports.DAL.Entities;

namespace Reports.Server.Services
{
    public interface IEmployeeService
    {
        Task<Employee> Create(string name, Role role, Guid supervisorId);

        IEnumerable<Employee> GetEmployees();

        Employee FindByName(string name);

        Employee FindById(Guid id);

        List<Employee> FindSubordinates(Guid id);
        
        Task<Employee> Delete(Guid id);

        Task<Employee> Update(Employee entity);
    }
}