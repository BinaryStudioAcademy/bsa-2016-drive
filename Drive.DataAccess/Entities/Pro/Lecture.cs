using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Drive.DataAccess.Interfaces;

namespace Drive.DataAccess.Entities.Pro
{
    public class Lecture : IEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public User Author { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime StartDate { get; set; }

        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        public AcademyProCourse Course { get; set; }

        public IList<ContentLink> ContentList { get; set; }

        public IList<CodeSample> CodeSamples { get; set; }
        public IList<HomeTask> HomeTasks { get; set; }
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; }
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime ModifiedAt { get; set; }
    }
}