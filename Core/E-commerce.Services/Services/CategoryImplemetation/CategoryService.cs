using AutoMapper;
using E_commerce.Abstraction.IService.Category;
using E_commerce.Domain.Contracts.UnitOfWorkPattern;
using E_commerce.Domain.Exceptions;
using E_commerce.Domain.Exceptions.NotFoundModels;
using E_commerce.Domain.Models.Category;
using E_commerce.Shared.Dto_s.Category;
using MimeKit.Cryptography;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Services.CategoryImplemetation
{
    public class CategoryService(IUnitOfWork _unitOfWork, IMapper _mapper) : ICategoryService
    {
        public async Task<CategoryDto> CreateCategoryAsync(CategoryToCreateDto categoryDto)
        {
            var categoryRepo = _unitOfWork.GetRepository<CategoryEntity, int>();
            var categoryEntity = _mapper.Map<CategoryEntity>(categoryDto);
            await categoryRepo.AddAsync(categoryEntity);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
                throw new BadRequestExceptionCustome("Failed to create category");
            return _mapper.Map<CategoryDto>(categoryEntity);

        }
        public async Task<IReadOnlyList<CategoryDto>> GetAllCategoriesAsync()
        {
            var categoryRepo = _unitOfWork.GetRepository<CategoryEntity, int>();
            var categories = await categoryRepo.GetAllAsync();
            var mappedCategories = _mapper.Map<IReadOnlyList<CategoryDto>>(categories);
            return mappedCategories;
        }
        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var categoryRepo = _unitOfWork.GetRepository<CategoryEntity, int>();
            var category = await categoryRepo.GetByIdAsync(id);
            if (category == null)
                throw new CategoryNotFoundException();
            var mappedCategory = _mapper.Map<CategoryDto>(category);
            return mappedCategory;
        }
        public async Task<CategoryDto> UpdateCategoryAsync(int id ,CategoryToUpdateDto categoryDto)
        {
            var categoryRepo = _unitOfWork.GetRepository<CategoryEntity, int>();
            var category = await categoryRepo.GetByIdAsync(id);
            
            if (category == null)
                throw new CategoryNotFoundException();

            _mapper.Map(categoryDto, category);

            categoryRepo.Update(category);

            var result = await _unitOfWork.SaveChangesAsync();
            
            if (result <= 0)
                throw new BadRequestExceptionCustome("Failed to update category");

            return _mapper.Map<CategoryDto>(category);

        }
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var categoryRepo = _unitOfWork.GetRepository<CategoryEntity, int>();
            var Category = await categoryRepo.GetByIdAsync(id);
            if (Category == null)
                throw new CategoryNotFoundException();
            categoryRepo.Delete(Category);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

    }
}
