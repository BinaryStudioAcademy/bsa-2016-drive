using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Drive.DataAccess.Entities;
using Drive.DataAccess.Interfaces;

namespace Drive.WebHost.Services
{
    public class SpaceService : ISpaceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SpaceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Space> GetAsync(int id)
        {
            return await _unitOfWork.Spaces.GetByIdAsync(id);
        }
        public async Task<IEnumerable<Space>> GetAllAsync()
        {
            return await _unitOfWork.Spaces.GetAllAsync();
        }
        public async Task CreateAsync(Space space)
        {
            _unitOfWork.Spaces.Create(space);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task UpdateAsync(Space space)
        {
            _unitOfWork.Spaces.Update(space);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            _unitOfWork.Spaces.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }
        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
        public void SaveChanges()
        {
            _unitOfWork.SaveChanges();
        }
    }
}