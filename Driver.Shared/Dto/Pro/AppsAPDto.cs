using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.Shared.Dto.Pro
{
    public class AppsAPDto
    {
        public int SpaceId { get; set; }
        public string Name { get; set; }
        public IEnumerable<AcademyProCourseDto> Courses { get; set; }
    }
}
