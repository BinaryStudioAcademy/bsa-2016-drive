using System;
using Drive.DataAccess.Entities;
using Driver.Shared.Dto.Users;
using System.Collections;
using System.Collections.Generic;

namespace Driver.Shared.Dto
{
    public class FileUnitDto
    {
        //public User Owner { get; set; }
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public FileType FileType { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public AuthorDto Author { get; set; }
        public int SpaceId { get; set; }
        public int ParentId { get; set; }
        public IList<ShareLinkDto> ShareLinks { get; set; }
        public DateTime LastModified { get; set; }
        public bool CanRead { get; set; }
        public bool CanModify { get; set; }
    }
}
