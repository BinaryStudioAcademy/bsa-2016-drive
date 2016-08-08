using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drive.DataAccess.Interfaces;

namespace Drive.DataAccess.Entities
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public string GlobalId { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public IList<Role> Roles { get; set; }

        public IList<DataUnit> ReadPermissionDataUnits { get; set; }

        public IList<DataUnit> ModifyPermissionDataUnits { get; set; }

        public IList<Space> ReadPermissionSpaces { get; set; }

        public IList<Space> ModifyPermissionSpaces { get; set; }
    }
}
