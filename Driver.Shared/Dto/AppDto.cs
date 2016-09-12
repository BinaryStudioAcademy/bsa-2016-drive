using Drive.DataAccess.Entities;
using System.Collections.Generic;

namespace Driver.Shared.Dto
{
    public class AppDto
    {
        public int SpaceId { get; set; }
        public SpaceType SpaceType { get; set; }
        public string Name { get; set; }
        public IEnumerable<FileUnitDto> Files { get; set; }
    }
}
