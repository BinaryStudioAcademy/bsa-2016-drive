using Drive.DataAccess.Entities;
using Driver.Shared.Dto.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.Shared.Dto.TrashBin
{
    public class TrashBinFileDto
    {
        public int Id { get; set; }
        public FileType FileType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public AuthorDto Author { get; set; }
        public int SpaceId { get; set; }
    }
}
