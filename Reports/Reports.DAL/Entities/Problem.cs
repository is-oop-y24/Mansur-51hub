using System;

namespace Reports.DAL.Entities
{
    public class Problem
    {
        public Guid Id { get; set; }
        public DateTime CreationTime { get; set; }
        public Status Status { get; set; }
        public Guid ResponsibleEmployeeId { get; set; }
        public byte[] Description { get; set; }

        public Problem(Guid id, Status status)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id), "Id is invalid");
            }

            Id = id;
            Status = status;
            CreationTime = DateTime.Now;
        }
    }
}
