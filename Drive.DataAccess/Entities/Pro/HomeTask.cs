using System;
using Drive.DataAccess.Interfaces;

namespace Drive.DataAccess.Entities.Pro
{
    public class HomeTask : IEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string Description { get; set; }
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime DeadlineDate { get; set; }
        public Lecture Lecture { get; set; }
    }
}