using System;
using System.Collections.Generic;
using Drive.DataAccess.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Drive.DataAccess.Entities.Pro
{
    public class AcademyProCourse : IEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime StartDate { get; set; }

        public IList<Lecture> Lectures { get; set; }

        public User Author { get; set; }

        public IList<Tag> Tags { get; set; }
        [Required]
        public FileUnit FileUnit { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
