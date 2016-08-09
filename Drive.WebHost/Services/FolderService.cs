using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Drive.DataAccess.Entities;
using Drive.DataAccess.Interfaces;

namespace Drive.WebHost.Services
{
    public class FolderService : IFolderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FolderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<FolderUnit>> GetAllAsync()
        {
            return await _unitOfWork.Folders.GetAllAsync();
        }

        public async Task<FolderUnit> GetAsync(int id)
        {
            return await _unitOfWork.Folders.GetByIdAsync(id);
        }

        public async Task<int> CreateAsync(FolderUnit folder)
        {
            folder.CreatedAt = DateTime.Now;
            folder.LastModified = DateTime.Now;
            folder.IsDeleted = false;

            _unitOfWork.Folders.Create(folder);
            await _unitOfWork.SaveChangesAsync();

            return folder.Id;
        }

        public async Task UpdateAsync(FolderUnit folder)
        {
            folder.LastModified = DateTime.Now;

            _unitOfWork.Folders.Update(folder);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            _unitOfWork.Folders.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}