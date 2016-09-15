using System.Collections.Generic;

namespace Driver.Shared.Dto
{
    public class CopyMoveContentDto
    {
        public int SpaceId { get; set; }
        public int NewParentId { get; set; }
        public IEnumerable<int> FilesId { get; set; }
        public IEnumerable<int> FoldersId { get; set; }
    }
}
