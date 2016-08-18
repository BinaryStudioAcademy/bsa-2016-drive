using System;
using Driver.Shared.Dto.Users;

namespace Driver.Shared.Dto
{
    public class FileUnitDto
    {
        //public User Owner { get; set; }
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string FileType { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public AuthorDto Author { get; set; }
        public int SpaceId { get; set; }
        public int ParentId { get; set; }
        public DateTime LastModified { get; set; }
    }
}
