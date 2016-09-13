using Drive.DataAccess.Entities.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.Shared.Dto.Events
{
    public class EventContentDto
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public string Link { get; set; }
        public ContentLinkType LinkType { get; set; }
    }
}
