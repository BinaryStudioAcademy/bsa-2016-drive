using Drive.DataAccess.Interfaces;

namespace Drive.DataAccess.Entities.Pro
{
    public class CodeSample : IEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }
    }
}