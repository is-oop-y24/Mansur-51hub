using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reports.DAL.Entities;
using Reports.Server.Database;

namespace Reports.Server.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ReportsDatabaseContext _context;

        public EmployeeService(ReportsDatabaseContext context) {
            _context = context;
        }

        public async Task<Employee> Create(string name, Role role, Guid supervisorId)
        {
            bool SupervisorExist = _context.Employees.Any(e => e.Id.Equals(supervisorId));

            if (SupervisorExist || role.Equals(Role.Teamlead))
            {
                var employee = new Employee(Guid.NewGuid(), name, role, supervisorId);
                _context.Employees.Add(employee);
                _context.Subordinates.Add(new Stuff(employee.Id));
                _context.EmployeeProblems.Add(new EmployeeTasks(employee.Id));

                if (!role.Equals(Role.Teamlead))
                {
                    _context.Subordinates.Find(supervisorId).Subordinates.Add(employee);
                }

                await _context.SaveChangesAsync();
                return employee;
            }

            return null;
        }

        public Employee FindByName(string name)
        {
            Employee requiredEmployee = _context.Employees.FirstOrDefault(p => p.Name.Equals(name));
            return requiredEmployee;
        }

        public Employee FindById(Guid id)
        {
            Employee requiredEmployee = _context.Employees.FirstOrDefault(p => p.Id.Equals(id));
            return requiredEmployee;
        }

        public async Task<Employee> Delete(Guid id)
        {
            Employee result = _context.Employees.FirstOrDefault(e => e.Id == id);

            if (result != null) {
                _context.Employees.Remove(FindById(id));
                await _context.SaveChangesAsync();
                return result;
            }

            return null;
        }

        public async Task<Employee> Update(Employee entity)
        {
            Employee employeeToUpdate = _context.Employees.FirstOrDefault(e => e.Id.Equals(entity.Id));
            
            if(employeeToUpdate != null)
            {
                employeeToUpdate.Name = entity.Name;
                await _context.SaveChangesAsync();
                return employeeToUpdate;
            }

            return null;
        }

        
        public IEnumerable<Employee> GetEmployees()
        {
           return _context.Employees.ToList();
        }

        public List<Employee> FindSubordinates(Guid id)
        {
            bool employeeExist = _context.Subordinates.Any(e => e.Id.Equals(id));

            if (!employeeExist)
            {
                return null;
            }

            return _context.Subordinates
                .Include(s => s.Subordinates)
                .First(e => e.Id.Equals(id))
                .Subordinates.ToList();
        }
    }
}