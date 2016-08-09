using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drive.DataAccess.Entities;

namespace Drive.DataAccess.Context
{
    public class DriveContext : DbContext
    {
        public DbSet<FolderUnit> Folders { get; set; }
        public DbSet<FileUnit> Files { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Space> Spaces { get; set; }
        public DbSet<Log> Logs { get; set; }


    }
}
