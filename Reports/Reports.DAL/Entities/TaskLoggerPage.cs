using System;
using System.Collections.Generic;

namespace Reports.DAL.Entities
{
    public class TaskLoggerPage
    {
        public Guid Id { get; set; }
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

        public TaskLoggerPage(Guid id)
        {
            Id = id;
            Messages.Add(new Message($"[{DateTime.Now:G}]: Task with id: {id} created"));
        }
    }
}
