using Drive.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.Shared.Dto
{
    public class ShareLinkDto
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public IEnumerable<FolderUnitDto> Folders { get; set; }
        public IEnumerable<FileUnitDto> Files { get; set; }
        public IEnumerable<ImageUnitDto> Images { get; set; }
    }
}
