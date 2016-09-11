using Drive.DataAccess.Entities;
using System.Collections.Generic;

namespace Driver.Shared.Dto.Pro
{
    public class AppsAPDto
    {
        public int SpaceId { get; set; }
        public SpaceType SpaceType { get; set; }
        public string Name { get; set; }
        public IEnumerable<AcademyProCourseDto> Courses { get; set; }
    }
}
