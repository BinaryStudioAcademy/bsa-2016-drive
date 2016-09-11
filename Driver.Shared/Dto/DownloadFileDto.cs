using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.Shared.Dto
{
    public class DownloadFileDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public MemoryStream Content { get; set; }
    }
}
