using AutoMapper;
using E_commerce.Abstraction.IService.Attachment;
using E_commerce.Abstraction.IService.Category;
using E_commerce.Domain.Contracts.UnitOfWorkPattern;
using E_commerce.Domain.Exceptions;
using E_commerce.Domain.Exceptions.NotFoundModels;
using E_commerce.Domain.Models.Category;
using E_commerce.Domain.Models.Product;
using E_commerce.Services.Specification.Category; 
using E_commerce.Services.Specification.Product;
using E_commerce.Shared.Dto_s.Category;

namespace E_commerce.Services.Services.CategoryImplemetation
{
    public class CategoryService(IUnitOfWork _unitOfWork, IMapper _mapper, IAttachmentService attachmentService) : ICategoryService
    {
        public async Task<CategoryDto> CreateCategoryAsync(CategoryToCreateDto categoryDto)
        {
            var categoryRepo = _unitOfWork.GetRepository<CategoryEntity, int>();
            var categoryEntity = _mapper.Map<CategoryEntity>(categoryDto);

            string imagePath = await attachmentService.UploadImageAsync(categoryDto.PictureUrl, "categories/images");

            categoryEntity.PictureUrl = imagePath;
            await categoryRepo.AddAsync(categoryEntity);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
                throw new BadRequestExceptionCustome("Failed to create category");
            return _mapper.Map<CategoryDto>(categoryEntity);
        }

        public async Task<IReadOnlyList<CategoryDto>> GetAllCategoriesAsync()
        {
            var categoryRepo = _unitOfWork.GetRepository<CategoryEntity, int>();

            var spec = new RegularCategorySpec();
            var categories = await categoryRepo.GetAllWithSpecAsync(spec);

            var mappedCategories = _mapper.Map<IReadOnlyList<CategoryDto>>(categories);
            return mappedCategories;
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var categoryRepo = _unitOfWork.GetRepository<CategoryEntity, int>();

            var spec = new RegularCategorySpec(id);
            var category = await categoryRepo.GetByIdWithSpecAsync(spec);

            if (category == null)
                throw new CategoryNotFoundException();

            var mappedCategory = _mapper.Map<CategoryDto>(category);
            return mappedCategory;
        }

        public async Task<CategoryDto> UpdateCategoryAsync(int id, CategoryToUpdateDto categoryDto)
        {
            var categoryRepo = _unitOfWork.GetRepository<CategoryEntity, int>();
            var category = await categoryRepo.GetByIdAsync(id);

            if (category == null)
                throw new CategoryNotFoundException();

            if (categoryDto.PictureUrl != null) 
            {
                if (!string.IsNullOrEmpty(category.PictureUrl))
                    await attachmentService.DeleteImage(category.PictureUrl);

                category.PictureUrl = await attachmentService.UploadImageAsync(categoryDto.PictureUrl, "categories/images");
            }
           

            _mapper.Map(categoryDto, category);

            category.Products = null;

            categoryRepo.Update(category);
            var result = await _unitOfWork.SaveChangesAsync();

            if (result <= 0)
                throw new BadRequestExceptionCustome("Failed to update category");

            return _mapper.Map<CategoryDto>(category);
        }
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var categoryRepo = _unitOfWork.GetRepository<CategoryEntity, int>();
            var category = await categoryRepo.GetByIdAsync(id);

            if (category == null)
                throw new CategoryNotFoundException();

            if (category.IsBaseGarment)
                throw new BadRequestExceptionCustome("Cannot delete this category because it is a core Base Garment category for customizations.");

            var productRepo = _unitOfWork.GetRepository<ProductEntity, int>();
            var spec = new ProductsByCategoryIdSpec(id); 
            var hasProducts = await productRepo.GetCountAsync(spec) > 0;

            if (hasProducts)
                throw new BadRequestExceptionCustome("لا يمكن حذف هذا القسم لأنه يحتوي على منتجات مرتبطة به. قم بنقل أو حذف المنتجات أولاً.");

            categoryRepo.Delete(category);
            var result = await _unitOfWork.SaveChangesAsync();

            if (result > 0)
            {
                if (!string.IsNullOrEmpty(category.PictureUrl))
                    await attachmentService.DeleteImage(category.PictureUrl);

                return true;
            }

            return false;
        }

        public async Task<IReadOnlyList<CategoryDto>> GetAllCategoriesForAdminAsync()
        {
            var categoryRepo = _unitOfWork.GetRepository<CategoryEntity, int>();
            var categories = await categoryRepo.GetAllAsync();
            if(categories == null || !categories.Any())
                throw new CategoryNotFoundException();
            return _mapper.Map<IReadOnlyList<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetCategoryByIdForAdminAsync(int id)
        {
            var categoryRepo = _unitOfWork.GetRepository<CategoryEntity, int>();

            var category = await categoryRepo.GetByIdAsync(id);

            if (category == null)
                throw new CategoryNotFoundException();

            var mappedCategory = _mapper.Map<CategoryDto>(category);
            return mappedCategory;
        }
    }
}