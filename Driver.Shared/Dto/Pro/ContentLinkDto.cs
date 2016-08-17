using Drive.DataAccess.Entities.Pro;

namespace Driver.Shared.Dto.Pro
{
    public class ContentLinkDto
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public string Link { get; set; }
        public LinkType LinkType { get; set; }
    }
}