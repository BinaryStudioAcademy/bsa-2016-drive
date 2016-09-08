using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Drive.DataAccess.Entities.Pro;
using Drive.DataAccess.Interfaces;
using Drive.Logging;
using Drive.WebHost.Services.Pro.Abstract;
using Driver.Shared.Dto.Pro;

namespace Drive.WebHost.Services.Pro
{
    public class HomeTasksService : IHomeTasksService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public HomeTasksService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<HomeTaskDto>> GetAllAsync()
        {
            var result = await _unitOfWork.HomeTasks.Query.Select(task => new HomeTaskDto
            {
                Id = task.Id,
                Description = task.Description,
                IsDeleted = task.IsDeleted,
                DeadlineDate = task.DeadlineDate
            }).ToListAsync();

            return result;
        }

        public async Task<HomeTaskDto> GetAsync(int id)
        {
            var result = await _unitOfWork.HomeTasks.Query.Where(c => c.Id == id).Select(task => new HomeTaskDto
            {
                Id = task.Id,
                Description = task.Description,
                IsDeleted = task.IsDeleted,
                DeadlineDate = task.DeadlineDate
            }).SingleOrDefaultAsync();

            return result;
        }

        public async Task<HomeTaskDto> CreateAsync(HomeTaskDto dto)
        {
            var link = new HomeTask
            {
                Description = dto.Description,
                IsDeleted = false,
                DeadlineDate = dto.DeadlineDate
            };

            _unitOfWork.HomeTasks.Create(link);
            await _unitOfWork.SaveChangesAsync();
            return dto;
        }

        public async Task<HomeTaskDto> UpdateAsync(int id, HomeTaskDto dto)
        {
            var homeTask = await _unitOfWork.HomeTasks.GetByIdAsync(id);

            homeTask.Description = dto.Description;
            homeTask.IsDeleted = dto.IsDeleted;
            homeTask.DeadlineDate = dto.DeadlineDate;

            await _unitOfWork.SaveChangesAsync();

            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            _unitOfWork.HomeTasks.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}