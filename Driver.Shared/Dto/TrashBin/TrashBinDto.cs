using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.Shared.Dto.TrashBin
{
    public class TrashBinDto
    {
        public int SpaceId { get; set; }
        public string Name { get; set; }
        public IEnumerable<TrashBinFolderDto> Folders { get; set; }
        public IEnumerable<TrashBinFileDto> Files { get; set; }
    }
}
