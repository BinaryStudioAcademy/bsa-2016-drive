using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.Shared.Dto
{
    public class AppDto
    {
        public int SpaceId { get; set; }
        public string Name { get; set; }
        public IEnumerable<FileUnitDto> Files { get; set; }
    }
}
