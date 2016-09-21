using Drive.DataAccess.Interfaces;
using System.Collections.Generic;

namespace Drive.DataAccess.Entities
{
    public class ShareLink : IEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string Link { get; set; }
        public IList<DataUnit> Content { get; set; }
    }
}
