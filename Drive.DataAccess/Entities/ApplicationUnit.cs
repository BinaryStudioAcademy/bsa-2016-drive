using System;
using Drive.DataAccess.Interfaces;

namespace Drive.DataAccess.Entities
{
    public abstract class ApplicationUnit : IEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }

        //public int UserId { get; set; }
        public User Owner { get; set; }
    }
}
