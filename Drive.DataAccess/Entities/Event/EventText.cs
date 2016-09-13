using Drive.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drive.DataAccess.Entities.Event
{
    class EventText : IEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string MessageText { get; set; }

        [Required]
        public Event Event { get; set; }
    }
}
