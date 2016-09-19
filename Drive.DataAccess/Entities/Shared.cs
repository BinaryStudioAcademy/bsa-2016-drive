using Drive.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Drive.DataAccess.Entities
{
    public class Shared: IEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        [Required]
        public DataUnit Content { get; set; }
        public User User { get; set; }
        public bool CanRead { get; set; }
        public bool CanModify { get; set; }
    }
}