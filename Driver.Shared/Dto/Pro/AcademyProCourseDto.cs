using System;
using System.Collections.Generic;

namespace Driver.Shared.Dto.Pro
{
    public class AcademyProCourseDto
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public IEnumerable<LectureDto> Lectures { get; set; }

        public FileUnitDto FileUnit { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
