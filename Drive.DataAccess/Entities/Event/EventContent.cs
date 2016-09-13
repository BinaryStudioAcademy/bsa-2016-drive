using Drive.DataAccess.Interfaces;

namespace Drive.DataAccess.Entities.Event
{
    public class EventContent : IEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public string Link { get; set; }
        public ContentLinkType LinkType { get; set; }
    }

    public enum ContentLinkType
    {
        None,
        Photo,
        Video,
    }
}
