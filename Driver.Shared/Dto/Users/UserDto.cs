using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.Shared.Dto
{
    public class UserDto
    {
        //public int LocalId { get; set; }
        public string serverUserId { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string department { get; set; }
        //public string avatar { get; set; }
    }
}
