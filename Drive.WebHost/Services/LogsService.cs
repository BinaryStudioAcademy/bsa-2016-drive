using Drive.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Driver.Shared.Dto;

namespace Drive.WebHost.Services
{
    public class LogsService : ILogsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LogsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<LogUnit> GetAsync(int id)
        {
            var data = await _unitOfWork.Logs.GetByIdAsync(id);
            
            return new LogUnit
            {
                Logged = data.Logged,
                Level = data.Level,
                Message = data.Message,
                Exception = data.Exception,
                CallerName = data.CallerName
            };
        }

        public async Task<IEnumerable<LogUnit>> GetAllAsync()
        {
            var data = await _unitOfWork.Logs.GetAllAsync();
            
            var dto = from d in data
                select new LogUnit()
                {
                    Logged = d.Logged,
                    Level = d.Level,
                    Message = d.Message,
                    Exception = d.Exception,
                    CallerName = d.CallerName
                };
            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            _unitOfWork.Logs.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<LogUnit>> SortSearchAsync(string sortOrder, string searchStr)
        {
            var data = await _unitOfWork.Logs.GetAllAsync();

            var dto = from d in data
                      select new LogUnit()
                      {
                          Logged = d.Logged,
                          Level = d.Level,
                          Message = d.Message,
                          Exception = d.Exception,
                          CallerName = d.CallerName
                      };

            if (!string.IsNullOrEmpty(searchStr))
            {
                dto = dto.Where(d => d.Level.Contains(searchStr));
            }

            switch (sortOrder)
            {
                case "Id":
                    dto = dto.OrderBy(d => d.Id);
                    break;
                case "Time":
                    dto = dto.OrderBy(d => d.Logged);
                    break;
                case "Log level":
                    dto = dto.OrderBy(d => d.Level);
                    break;
                case "Log message":
                    dto = dto.OrderBy(d => d.Message);
                    break;
                case "Exception":
                    dto = dto.OrderBy(d => d.Exception);
                    break;
                case "Called from":
                    dto = dto.OrderBy(d => d.CallerName);
                    break;
                default:
                    dto = dto.OrderBy(d => d.Id);
                    break;
            }

            return dto;
        }

        public async Task<IEnumerable<LogUnit>> FromToAsync(int from, int to)
        {
            var data = await _unitOfWork.Logs.GetAllAsync();

            var dto = (from d in data                                        
                      select new LogUnit()
                      {
                          Logged = d.Logged,
                          Level = d.Level,
                          Message = d.Message,
                          Exception = d.Exception,
                          CallerName = d.CallerName
                      })
                      .Skip(from)
                      .Take(to);

            return dto;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}