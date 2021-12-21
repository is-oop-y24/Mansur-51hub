using System;
using System.Collections.Generic;

namespace Reports.DAL.Entities
{
    public class Stuff
    {
        public Guid Id { get; set; }
        public virtual ICollection<Employee> Subordinates { get; set; } = new List<Employee>();

        public Stuff(Guid id)
        {
            Id = id;
        }
    }
}
