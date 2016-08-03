using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drive.Identity
{
    public class BSIdentityManager : GenericIdentityManager<BSIdentity>
    {
        public int UserId => Identity.UserId;
        public string Token => Identity.Token;
        public string Name => Identity.Name;
    }
}
