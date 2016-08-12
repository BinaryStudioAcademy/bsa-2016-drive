using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.Shared.Dto
{
    public class FolderUnitDto
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }
        public AuthorDto Author { get; set; }
    }
}
