using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Driver.Shared.Dto;

namespace Drive.WebHost.Services
{
    public interface ILogsService
    {
        Task<IEnumerable<LogUnit>> GetAllAsync();

        Task<LogUnit> GetAsync(int id);

        Task DeleteAsync(int id);

        Task<IEnumerable<LogUnit>> SortSearchAsync(string sortOrder, string searchStr);

        void Dispose();
    }
}