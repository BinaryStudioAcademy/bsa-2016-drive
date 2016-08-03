using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Drive.Identity
{
    public class BSIdentity : IIdentity
    {
        public string Name { get; }
        public string Token { get; set; }
        public int UserId { get; set; }
        public string AuthenticationType { get; }
        public bool IsAuthenticated { get; }

        // TODO: Add fields for identity
    }
}
