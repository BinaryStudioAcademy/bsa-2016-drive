using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drive.DataAccess.Interfaces;

namespace Drive.DataAccess.Entities
{
    public class Log : IEntity
    {

        public int Id { get; set; }

        public DateTime Logged { get; set; }

        public string Level { get; set; }

        public string Message { get; set; }
        public string Exception { get; set; }
        public string CallerName { get; set; }

        [NotMapped]
        public bool IsDeleted { get; set; }

    }
}
