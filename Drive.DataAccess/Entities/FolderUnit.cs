using System;
using System.Collections.Generic;

namespace Drive.DataAccess.Entities
{
    public class FolderUnit : DataUnit
    {
        public IList<DataUnit> DataUnits { get; set; }
    }
}
