using System;


namespace Reports.DAL.Entities
{
    public class Message
    {
        public Message(string information)
        {
            Id = Guid.NewGuid();
            Information = information;
        }

        public Guid Id { get; set; }
        public string Information { get; set; }
    }
}
