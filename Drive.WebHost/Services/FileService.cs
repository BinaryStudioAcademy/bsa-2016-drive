﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Driver.Shared.Dto;
using Drive.DataAccess.Entities;
using Drive.DataAccess.Interfaces;

namespace Drive.WebHost.Services
{
    public class FileService : IFileService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<FileUnitDto>> GetAllAsync()
        {
            var data = await _unitOfWork.Files.GetAllAsync();

            var dto = from d in data
                      select new FileUnitDto()
                      {
                          Id = d.Id,
                          IsDeleted = d.IsDeleted,
                          FileType = d.FileType,
                          Name = d.Name,
                          Link = d.Link,
                          Description = d.Description,
                          SpaceId = d.Space.Id
                      };

            return dto;
        }

        public async Task<FileUnitDto> GetAsync(int id)
        {
            var file = await _unitOfWork.Files.GetByIdAsync(id);

            return new FileUnitDto
            {
                Id = file.Id,
                IsDeleted = file.IsDeleted,
                FileType = file.FileType,
                Name = file.Name,
                Description = file.Description,
                Link = file.Link
            };
        }

        public async Task<FileUnitDto> CreateAsync(FileUnitDto dto)
        {
            var space = await _unitOfWork.Spaces.GetByIdAsync(dto.SpaceId);
            var parentFolder = await _unitOfWork.Folders.GetByIdAsync(dto.ParentId);

            var file = new FileUnit()
            {
                Name = dto.Name,
                Link = dto.Link,
                FileType = dto.FileType,
                Description = dto.Description,

                CreatedAt = DateTime.Now,
                LastModified = DateTime.Now,
                IsDeleted = false,
                Space = space,
                FolderUnit = parentFolder
            };

            _unitOfWork.Files.Create(file);
            await _unitOfWork.SaveChangesAsync();

            dto.Id = file.Id;
            dto.CreatedAt = file.CreatedAt;
            dto.LastModified = file.LastModified;

            return dto;
        }

        public async Task<FileUnitDto> UpdateAsync(int id, FileUnitDto dto)
        {

            var file = await _unitOfWork.Files.GetByIdAsync(id);

            file.Name = dto.Name;
            file.FileType = dto.FileType;
            file.Description = dto.Description;
            file.IsDeleted = dto.IsDeleted;
            file.LastModified = DateTime.Now;
            file.Link = dto.Link;

            await _unitOfWork.SaveChangesAsync();

            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            _unitOfWork.Files.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}