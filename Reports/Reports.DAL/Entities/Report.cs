using System;

namespace Reports.DAL.Entities
{
    public class Report
    {
        public Guid Id { get; set; }
        public byte[] Reports { get; set; }

        public Report(Guid id)
        {
            Id = id;
        }
    }
}
