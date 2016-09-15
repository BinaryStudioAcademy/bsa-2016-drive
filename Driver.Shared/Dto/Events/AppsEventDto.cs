using Drive.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.Shared.Dto.Events
{
    public class AppsEventDto
    {
        public int SpaceId { get; set; }
        public SpaceType SpaceType { get; set; }
        public string Name { get; set; }
        public IEnumerable<EventDto> Events { get; set; }
    }
}
