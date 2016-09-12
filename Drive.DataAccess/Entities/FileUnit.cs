using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drive.DataAccess.Entities
{
    public class FileUnit : DataUnit
    {
        public FileType FileType { get; set; }
        public string Link { get; set; }
    }

    public enum FileType
    {
        Undefined,
        Document,
        Sheets,
        Slides,
        Trello,
        Link,
        Physical,
        AcademyPro,
        Event
    }
}
