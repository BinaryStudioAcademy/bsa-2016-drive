using Drive.DataAccess.Entities.Event;
using Driver.Shared.Dto.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.Shared.Dto.Events
{
    public class EventDto
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public EventType EventType { get; set; }
        public DateTime EventDate { get; set; }
        public IList<EventContentDto> ContentVideoLinks { get; set; }
        public IList<EventContentDto> ContentSimpleLinks { get; set; }
        public IList<EventContentDto> ContentPhotos { get; set; }
        public IList<EventContentDto> ContentTexts { get; set; }
        public FileUnitDto FileUnit { get; set; }
        public AuthorDto Author { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
