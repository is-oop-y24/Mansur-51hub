using System;
using System.Collections.Generic;

namespace Reports.DAL.Entities
{
    public class EmployeeTasks
    {
        public Guid Id { get; set; }
        public virtual ICollection<Problem> Problems { get; set; } = new List<Problem>();

        public EmployeeTasks(Guid id)
        { 
            Id = id;
        }
    }
}
