using System.Collections.Generic;
using System.Threading.Tasks;
using Driver.Shared.Dto.Pro;

namespace Drive.WebHost.Services.Pro.Abstract
{
    public interface IAcademyProCourseService : IBasicService<AcademyProCourseDto>
    {
        Task<IEnumerable<AppsAPDto>> SearchCourses(string text);
    }
}
