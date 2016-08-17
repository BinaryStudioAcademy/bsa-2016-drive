using System.Collections.Generic;
using System.Threading.Tasks;
using Driver.Shared.Dto.Pro;

namespace Drive.WebHost.Services.Pro.Abstract
{
    public interface IAcademyProCourseService
    {
        Task<IEnumerable<AcademyProCourseDto>> GetAllAsync();
        Task<AcademyProCourseDto> GetAsync(int id);
        Task<int> CreateAsync(AcademyProCourseDto dto);
        Task UpdateAsync(int id, AcademyProCourseDto dto);
        Task DeleteAsync(int id);
        void Dispose();
    }
}
