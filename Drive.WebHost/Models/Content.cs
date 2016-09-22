using Driver.Shared.Dto;
using System.Collections.Generic;
namespace Drive.WebHost.Models
{
    public class SharedContent
    {
        public IEnumerable<FolderUnitDto> Folders { get; set; }
        public IEnumerable<FileUnitDto> Files { get; set; }
    }
}