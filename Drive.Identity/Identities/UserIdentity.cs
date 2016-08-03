using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Drive.Identity.Identities
{
    class UserIdentity : IUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public UserIdentity(string userName)
        {
            Id = Guid.NewGuid().ToString();
            UserName = userName;
        }
    }
}
