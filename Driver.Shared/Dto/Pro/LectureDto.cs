using System;
using System.Collections.Generic;
using Driver.Shared.Dto.Users;

namespace Driver.Shared.Dto.Pro
{
    public class LectureDto
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public AuthorDto Author { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public IEnumerable<ContentLinkDto> VideoLinks { get; set; }

        public IEnumerable<ContentLinkDto> SlidesLinks { get; set; }

        public IEnumerable<ContentLinkDto> SampleLinks { get; set; }

        public IEnumerable<ContentLinkDto> UsefulLinks { get; set; }

        public IEnumerable<ContentLinkDto> RepositoryLinks { get; set; }

        public IEnumerable<CodeSampleDto> CodeSamples { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}