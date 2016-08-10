using Drive.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.Shared.Dto
{
    public class FileUnitDto
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public FileType FyleType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
