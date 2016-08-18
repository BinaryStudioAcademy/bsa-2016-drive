using System;
using System.Collections.Generic;
using Drive.DataAccess.Interfaces;

namespace Drive.DataAccess.Entities.Pro
{
    public class Lecture : IEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public User Author { get; set; }
        public IList<ContentLink> ContentList { get; set; }

        public IList<Tag> Tags { get; set; }
        public IList<CodeSample> CodeSamples { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}