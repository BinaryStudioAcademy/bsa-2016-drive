using System;
using System.Collections.Generic;

namespace Driver.Shared.Dto.Pro
{
    public class LectureDto
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public UserDto Author { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public IEnumerable<ContentLinkDto> ContentList { get; set; }

        public IEnumerable<TagDto> Tags { get; set; }
        public IEnumerable<CodeSampleDto> CodeSamples { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}