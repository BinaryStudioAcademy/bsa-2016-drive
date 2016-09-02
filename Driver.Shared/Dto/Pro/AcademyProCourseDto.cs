using System;
using System.Collections.Generic;
using Driver.Shared.Dto.Users;

namespace Driver.Shared.Dto.Pro
{
    public class AcademyProCourseDto
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime StartDate { get; set; }

        public AuthorDto Author { get; set; }

        public IEnumerable<LectureDto> Lectures { get; set; }

        public IEnumerable<TagDto> Tags { get; set; }

        public FileUnitDto FileUnit { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
