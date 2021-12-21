using System;


namespace Reports.DAL.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Role Role { get; set; }
        public Guid SupervisorId { get; set; }

        public Employee(Guid id, string name, Role role, Guid supervisorId)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id), "Id is invalid");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "Name is invalid");
            }

            if(supervisorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(supervisorId), "Id is invalid");
            }

            Id = id;
            Name = name;
            Role = role;
            SupervisorId = supervisorId;
        }
    }
}