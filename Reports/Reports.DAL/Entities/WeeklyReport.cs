using System;
using System.Collections.Generic;

namespace Reports.DAL.Entities
{
    public class WeeklyReport
    {
        public Guid Id { get; set; }
        public virtual ICollection<Report> Reports {get; set;} = new List<Report>();
        public byte[] Description { get; set; }

        public WeeklyReport()
        {
            Id = Guid.NewGuid();
        }
    }
}
