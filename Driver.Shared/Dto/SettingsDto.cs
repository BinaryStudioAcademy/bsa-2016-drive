using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drive.DataAccess.Entities;

namespace Driver.Shared.Dto
{
    public class SettingsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public int MaxFilesQuantity { get; set; }

        public int MaxFileSize { get; set; }
    }
}
