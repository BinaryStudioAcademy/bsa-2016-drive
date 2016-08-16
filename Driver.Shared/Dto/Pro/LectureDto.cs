using System;
using System.Collections.Generic;

namespace Driver.Shared.Dto.Pro
{
    public class LectureDto
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public UserDto Author { get; set; }
        public IList<ContentLinkDto> ContentList { get; set; }

        public IList<TagDto> Tags { get; set; }
        public IList<CodeSampleDto> CodeSamples { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}