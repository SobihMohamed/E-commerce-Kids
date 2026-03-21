using E_commerce.Shared.Dto_s.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Abstraction.IService.Category
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> GetCategoryByIdAsync(int id);

        Task<CategoryDto> CreateCategoryAsync(CategoryToCreateDto categoryDto);
        Task<CategoryDto> UpdateCategoryAsync(int id ,CategoryToUpdateDto categoryDto);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
