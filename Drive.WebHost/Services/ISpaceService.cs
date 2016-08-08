using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drive.DataAccess.Entities;
using Drive.DataAccess.Interfaces;

namespace Drive.WebHost.Services
{
    public interface ISpaceService
    {
        IRepository<Space> SpaceRepository();
        void SaveChanges();
        void Dispose();
    }
}
