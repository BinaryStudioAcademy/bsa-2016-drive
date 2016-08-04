using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drive.DataAccess.Entities
{
    public class Space : ApplicationUnit
    {
        public int MaxFilesQuantity { get; set; }
        public int MaxFileSize { get; set; }
        public IList<DataUnit> ContentList { get; set; }

        public IList<User> ReadPermittedUsers { get; set; }
        public IList<User> ModifyPermittedUsers { get; set; }
        public IList<Role> ReadPermittedRoles { get; set; }
        public IList<Role> MorifyPermittedRoles { get; set; }
    }
}
