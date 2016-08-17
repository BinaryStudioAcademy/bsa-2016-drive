using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Drive.DataAccess.Entities.Pro;
using Drive.DataAccess.Interfaces;
using Drive.Logging;
using Drive.WebHost.Services.Pro.Abstract;
using Driver.Shared.Dto.Pro;

namespace Drive.WebHost.Services.Pro
{
    public class CodeSamplesService : ICodeSamplesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public CodeSamplesService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<CodeSampleDto>> GetAllAsync()
        {
            var result = await _unitOfWork.CodeSamples.Query.Select(sample => new CodeSampleDto
            {
                Id = sample.Id,
                Name = sample.Name,
                IsDeleted = sample.IsDeleted,
                Code = sample.Code
            }).ToListAsync();

            return result;
        }

        public async Task<CodeSampleDto> GetAsync(int id)
        {
            var result = await _unitOfWork.CodeSamples.Query.Where(c => c.Id == id).Select(sample => new CodeSampleDto
            {
                Id = sample.Id,
                Name = sample.Name,
                IsDeleted = sample.IsDeleted,
                Code = sample.Code
            }).SingleOrDefaultAsync();

            return result;
        }

        public async Task<int> CreateAsync(CodeSampleDto dto)
        {
            var link = new CodeSample
            {
                Name = dto.Name,
                IsDeleted = false,
                Code = dto.Code
            };

            _unitOfWork.CodeSamples.Create(link);
            await _unitOfWork.SaveChangesAsync();
            return link.Id;
        }

        public async Task UpdateAsync(int id, CodeSampleDto dto)
        {
            var codeSample = await _unitOfWork.CodeSamples.GetByIdAsync(id);

            codeSample.Name = dto.Name;
            codeSample.IsDeleted = dto.IsDeleted;
            codeSample.Code = dto.Code;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            _unitOfWork.CodeSamples.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}