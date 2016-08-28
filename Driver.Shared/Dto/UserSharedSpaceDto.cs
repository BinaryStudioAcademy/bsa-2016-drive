using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.Shared.Dto
{
    public class UserSharedSpaceDto
    {
        public string GlobalId { get; set; }
        public bool IsDeleted { get; set; }
        public bool CanRead { get; set; }
        public bool CanModify { get; set; }
    }
}
