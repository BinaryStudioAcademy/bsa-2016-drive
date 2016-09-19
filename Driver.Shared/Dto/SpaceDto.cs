using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drive.DataAccess.Entities;

namespace Driver.Shared.Dto
{
    public class SpaceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SpaceType Type { get; set; }

        public string Description { get; set; }

        public int MaxFilesQuantity { get; set; }

        public int MaxFileSize { get; set; }

        public IEnumerable<FileUnitDto> Files { get; set; }

        public IEnumerable<FolderUnitDto> Folders {get;set;}

        public User Owner { get; set; }

        public bool CanModifySpace { get; set; }
    }
}
