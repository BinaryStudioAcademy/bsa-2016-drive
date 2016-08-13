using Drive.DataAccess.Entities;
using System;

namespace Driver.Shared.Dto
{
    public class FileUnitDto
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public FileType FyleType { get; set; }
        public string Name { get; set; }
       // public Uri Link { get; set; }
        public string Description { get; set; }
    }
}
