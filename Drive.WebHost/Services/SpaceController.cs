using Drive.DataAccess.Entities;
using Drive.DataAccess.Interfaces;

namespace Drive.WebHost.Services
{
    public class SpaceService : ISpaceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IRepository<Space> _spaceRepository;

        public SpaceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public IRepository<Space> SpaceRepository()
        {
            return _spaceRepository = _unitOfWork.Spaces;
        }

        public void SaveChanges()
        {
            _unitOfWork.SaveChanges();
        }
    }
}