using System;

namespace Driver.Shared.Dto.Pro
{
    public class HomeTaskDto
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string Description { get; set; }
        public DateTime DeadlineDate { get; set; }
    }
}
