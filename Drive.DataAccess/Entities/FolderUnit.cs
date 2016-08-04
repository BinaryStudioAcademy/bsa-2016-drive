using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drive.DataAccess.Entities
{
    public class FolderUnit : DataUnit
    {
        public IList<DataUnit> DataUnits { get; set; }
    }
}
