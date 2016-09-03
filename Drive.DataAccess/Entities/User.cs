using System;
using System.Collections.Generic;
using Drive.DataAccess.Interfaces;

namespace Drive.DataAccess.Entities
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public string GlobalId { get; set; }

        public IList<Role> Roles { get; set; }

        public IList<DataUnit> ReadPermissionDataUnits { get; set; }

        public IList<DataUnit> ModifyPermissionDataUnits { get; set; }

        public IList<Space> ReadPermissionSpaces { get; set; }

        public IList<Space> ModifyPermissionSpaces { get; set; }
        public IList<Shared> SharedFiles { get; set; }
    }
}
