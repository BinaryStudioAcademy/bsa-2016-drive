using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drive.DataAccess.Interfaces;

namespace Drive.DataAccess.Entities
{
    public class Role:IEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public IList<User> Users { get; set; }

        public IList<DataUnit> ReadPermissionDataUnits { get; set; }
        public IList<DataUnit> ModifyPermissionDataUnits { get; set; }

        public IList<Space> ReadPermissionSpaces { get; set; }
        public IList<Space> ModifyPermissionSpaces { get; set; }
    }
}
