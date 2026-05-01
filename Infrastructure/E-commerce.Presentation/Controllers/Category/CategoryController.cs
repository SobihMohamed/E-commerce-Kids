using E_commerce.Abstraction.IService.Category;
using E_commerce.Shared.Common.Responses; 
using E_commerce.Shared.Dto_s.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Presentation.Controllers.Category
{
    public class CategoryController : AppBaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // 1. Get All Categories (Public)
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<CategoryDto>>), 200)]
        public async Task<ActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Success(categories, "Categories retrieved successfully");
        }

        // 2. Get Category By ID (Public)
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<CategoryDto>), 200)]
        public async Task<ActionResult> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            return Success(category, "Category retrieved successfully");
        }

        // 3. Create a new category (Admin Only)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<CategoryDto>), 201)] 
        public async Task<ActionResult> CreateCategory([FromForm] CategoryToCreateDto dto)
        {
            var category = await _categoryService.CreateCategoryAsync(dto);
            return Created(category, "Category created successfully");
        }

        // 4. Update category (Admin Only)
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<CategoryDto>), 200)]
        public async Task<ActionResult> UpdateCategory(int id, [FromForm] CategoryToUpdateDto dto)
        {
            var category = await _categoryService.UpdateCategoryAsync(id, dto);
            return Success(category, "Category updated successfully");
        }

        // 5. Delete category (Admin Only)
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)] 
        [ProducesResponseType(typeof(ApiResponse<object>), 400)] 
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);

            if (!result)
                return BadRequestError("Failed to delete the category");

            return Success("Category deleted successfully");
        }
    }
}