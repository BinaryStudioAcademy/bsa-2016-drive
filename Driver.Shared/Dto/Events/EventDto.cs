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
        public string EventType { get; set; }
        public DateTime EventDate { get; set; }
        public IEnumerable<EventContentDto> ContentList { get; set; }
        public FileUnitDto FileUnit { get; set; }
        public AuthorDto Author { get; set; }
    }
}
