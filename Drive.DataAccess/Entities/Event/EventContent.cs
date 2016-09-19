using System;
using Drive.DataAccess.Interfaces;

namespace Drive.DataAccess.Entities.Event
{
    public class EventContent : IEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public string Content { get; set; }
        public ContentType ContentType { get; set; }
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; }
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime LastModified { get; set; }
        public Event Event { get; set; }
        public int Order { get; set; }

    }

    public enum ContentType
    {
        None,
        Text,
        Photo,
        Video,
        Link
    }
}
