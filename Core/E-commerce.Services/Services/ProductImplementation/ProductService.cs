using AutoMapper;
using E_commerce.Abstraction.IService.Attachment;
using E_commerce.Abstraction.IService.Product;
using E_commerce.Domain.Contracts.UnitOfWorkPattern;
using E_commerce.Domain.Exceptions;
using E_commerce.Domain.Models.Product;
using E_commerce.Services.Specification.Product;
using E_commerce.Shared.Common.Pagination;
using E_commerce.Shared.Common.Params.Product;
using E_commerce.Shared.Dto_s.Product;
using E_commerce.Shared.EnumsHelper.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Services.ProductImplementation
{
    public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper , IAttachmentService attachmentService) : IProductService
    {
        public async Task<PaginationResponse<ProductDto>> GetAllProductsAsync(ProductSpecParams specParams)
        {
            var productRepo = _unitOfWork.GetRepository<ProductEntity, int>();

            // 1. spec for filtration and pagination
            var spec = new ProductFiltrationSpec(specParams);
            var products = await productRepo.GetAllWithSpecAsync(spec);

            // 2.get count of total items for pagination
            var countSpec = new ProductWithFiltersForCountSpec(specParams);
            var totalItems = await productRepo.GetCountAsync(countSpec);

            // 3. Mapping
            var mappedProducts = _mapper.Map<IReadOnlyList<ProductDto>>(products);

            // 4. Return paginated response
            return new PaginationResponse<ProductDto>(
                specParams.PageIndex,
                specParams.PageSize,
                totalItems,
                mappedProducts
            );
        }
        public async Task<ProductDetailsDto> GetProductDetailsByIdAsync(int id, bool? isBaseGarment = null)
        {
            var productRepo = _unitOfWork.GetRepository<ProductEntity, int>();

            var spec = new ProductByIdSpec(id, isBaseGarment);

            var product = await productRepo.GetByIdWithSpecAsync(spec);
            if (product == null)
                throw new NotFoundExceptionCustome($"Product with ID {id} not found.");

            return _mapper.Map<ProductDetailsDto>(product);
        }
        public async Task<ProductDetailsDto> CreateProductAsync(ProductToCreateDto productDto)
        {
            // 1. get product repo 
            var productRepo = _unitOfWork.GetRepository<ProductEntity, int>();

            // 2. map the incoming DTO to the ProductEntity
            var productEntity = _mapper.Map<ProductEntity>(productDto);

            // create a unique folder name for the product images using a GUID
            string productFolderId = Guid.NewGuid().ToString();

            // 3. add main image
            string mainImagePath = Path.Combine("Products", productFolderId, "Main");
            var fullMainImagePath = await attachmentService.UploadImageAsync(productDto.MainImageUrl,mainImagePath);
            productEntity.MainImageUrl = fullMainImagePath;

            // 4- add additional images if provided
            string galleryImagePath = Path.Combine("Products", productFolderId, "Gallary");
            if (productDto.ImageUrls!=null && productDto.ImageUrls.Count() > 0)
            {
                foreach (var image in productDto.ImageUrls)
                {
                    var realGalleryPath = await attachmentService.UploadImageAsync(image, galleryImagePath);
                    productEntity.Images.Add(new ProductImageEntity { ImageUrl = realGalleryPath });
                }
            }
            // 5. add the product to the database
            await productRepo.AddAsync(productEntity);
            var result = await _unitOfWork.SaveChangesAsync();

            if (result <= 0)
                throw new BadRequestExceptionCustome("failed to add the product");

            // 6. Return the full details DTO
            return await GetProductDetailsByIdAsync(productEntity.Id);

        }
        public async Task<ProductDetailsDto> UpdateProductAsync(int id, ProductToUpdateDto productDto)
        {
            var productRepo = _unitOfWork.GetRepository<ProductEntity, int>();
            var spec = new ProductByIdSpec(id);
            var existingProduct = await productRepo.GetByIdWithSpecAsync(spec);
            if (existingProduct == null)
                throw new NotFoundExceptionCustome($"product not found");

            // 1 - get productFolder Name 
            var ProductFolderName = GetProductFolderId(existingProduct);

            existingProduct.Name = productDto.Name;
            existingProduct.Description = productDto.Description;
            existingProduct.Price = productDto.Price;
            existingProduct.CategoryId = productDto.CategoryId;
            existingProduct.TargetGender = productDto.TargetGender;
            existingProduct.UpdatedAt = DateTime.UtcNow;

            // 3 - handel the images of the product 
            await HandleProductImagesUpdateAsync(productDto, existingProduct, ProductFolderName);

            // 4 - handel the variants of the product
            HandleProductVariantsUpdate(productDto, existingProduct);

            // 5 - save the changes to the database
            // 5 - save the changes to the database
            productRepo.Update(existingProduct);

            try
            {
                var result = await _unitOfWork.SaveChangesAsync();
                if (result <= 0)
                    throw new BadRequestExceptionCustome("Failed to update product");
            }
            catch (Exception ex)
            {
                throw new BadRequestExceptionCustome(
                    "لا يمكن حذف هذا الصنف  لأنه مرتبط بطلبات وفواتير سابقة للعملاء. يمكنك تصفير المخزون المتاح بدلاً من حذفه.");
            }

            return await GetProductDetailsByIdAsync(existingProduct.Id);
        }
        public async Task<ProductDetailsDto> GetCustomizationProductAsync(TargetGender gender)
        {
            var productRepo = _unitOfWork.GetRepository<ProductEntity, int>();

            var spec = new BaseGarmentByGenderSpec(gender);

            var product = await productRepo.GetByIdWithSpecAsync(spec);

            if (product == null)
                throw new NotFoundExceptionCustome($"No base customization garment found for gender: {gender}");

            return _mapper.Map<ProductDetailsDto>(product);
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            var productRepo = _unitOfWork.GetRepository<ProductEntity, int>();
            var spec = new ProductByIdSpec(id);
            var existingProduct = await productRepo.GetByIdWithSpecAsync(spec);

            if (existingProduct == null) return false;

            if (existingProduct.IsBaseGarment)
                throw new BadRequestExceptionCustome("لا يمكن حذف المنتج الأساسي المخصص للطباعة. يمكنك فقط إدارة أصنافه (Variants).");

            if (existingProduct.Variants != null)
            {
                foreach (var variant in existingProduct.Variants)
                {
                    // بنصفر الألوان والمقاسات عشان EF Core ميمشيش وراها ويعمل قفلة التتبع
                    variant.Color = null;
                    variant.Size = null;
                    variant.Product = null;
                }
            }

            productRepo.Delete(existingProduct);

            try
            {
                var result = await _unitOfWork.SaveChangesAsync();

                if (result > 0)
                {
                    if (!string.IsNullOrEmpty(existingProduct.MainImageUrl))
                        await attachmentService.DeleteImage(existingProduct.MainImageUrl);

                    if (existingProduct.Images != null && existingProduct.Images.Any())
                    {
                        foreach (var img in existingProduct.Images)
                        {
                            await attachmentService.DeleteImage(img.ImageUrl);
                        }
                    }
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                throw new BadRequestExceptionCustome(
                    "لا يمكن حذف هذا المنتج لأنه مرتبط بطلبات وفواتير سابقة للعملاء. يمكنك تعطيله أو تصفير مخزونه بدلاً من حذفه.");
            }
        }
        public async Task<bool> ToggleProductActivityAsync(int id)
        {
            var productRepo = _unitOfWork.GetRepository<ProductEntity, int>();
            var product = await productRepo.GetByIdAsync(id);

            if (product == null) return false;

            product.IsActive = !product.IsActive;

            productRepo.Update(product);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        #region Handler Methods
        private void HandleProductVariantsUpdate(ProductToUpdateDto productDto, ProductEntity existingProduct)
        {
            // ===================================================================
            // 1. مسح الـ Variants القديمة (مع تنظيف الميموري لمنع إيرور 500)
            // ===================================================================
            if (productDto.Variants != null)
            {
                var incomingVariantIds = productDto.Variants.Select(v => v.Id).ToList();
                var existingVariantIds = existingProduct.Variants.Select(v => v.Id).ToList();

                var variantsToDelete = existingProduct.Variants.Where(ev => !incomingVariantIds.Contains(ev.Id)).ToList();

                var variantRepo = _unitOfWork.GetRepository<ProductVariantEntity, int>();
                foreach (var variant in variantsToDelete)
                {
                    variant.Color = null;
                    variant.Size = null;
                    variant.Product = null;

                    existingProduct.Variants.Remove(variant);

                    variantRepo.Delete(variant);
                }
            }

            // ===================================================================
            // 2. إضافة أو تحديث الـ Variants
            // ===================================================================
            if (productDto.Variants != null)
            {
                foreach (var variantDto in productDto.Variants)
                {
                    if (variantDto.Id.HasValue)
                    {
                        var existingVariant = existingProduct.Variants.FirstOrDefault(v => v.Id == variantDto.Id);
                        if (existingVariant != null)
                        {
                            if (variantDto.ColorId <= 0 || variantDto.SizeId <= 0)
                                throw new BadRequestExceptionCustome("Invalid Color ID or Size ID provided for the variant.");

                            _mapper.Map(variantDto, existingVariant);

                            existingVariant.Color = null;
                            existingVariant.Size = null;
                            existingVariant.Product = null;
                        }
                    }
                    else
                    {
                        var newVariant = _mapper.Map<ProductVariantEntity>(variantDto);

                        newVariant.Color = null;
                        newVariant.Size = null;
                        newVariant.Product = null;

                        existingProduct.Variants.Add(newVariant);
                    }
                }
            }
        }
        private async Task HandleProductImagesUpdateAsync(ProductToUpdateDto productDto, ProductEntity existingProduct, string productFolderName)
        {
            if (productDto.MainImageUrl != null) // check if new main image is provided
            {
                if (!string.IsNullOrEmpty(existingProduct.MainImageUrl)) // check if there is an existing main image to delete
                    await attachmentService.DeleteImage(existingProduct.MainImageUrl); // delete old main image
                var mainImagePath = Path.Combine("Products", productFolderName, "Main"); // define path for main image
                var fullMainImagePath = await attachmentService.UploadImageAsync(productDto.MainImageUrl, mainImagePath); // upload new main image and get its path
                existingProduct.MainImageUrl = fullMainImagePath; // update main image URL in the product entity
            }
            if (productDto.DeletedImageIds != null && productDto.DeletedImageIds.Any())
            {
                var imagesToDelete = existingProduct.Images.Where(
                    img => productDto.DeletedImageIds.Contains(img.Id)
                    ).ToList();
                foreach (var img in imagesToDelete)
                {
                    await attachmentService.DeleteImage(img.ImageUrl); // delete image from storage
                    existingProduct.Images.Remove(img); // remove image from product entity
                }
            }
            if (productDto.ImageUrls != null && productDto.ImageUrls.Any())
            {
                string galleryImagePath = Path.Combine("Products", productFolderName, "Gallary");
                foreach (var image in productDto.ImageUrls)
                {
                    var realGalleryPath = await attachmentService.UploadImageAsync(image, galleryImagePath); // upload new gallery image
                    existingProduct.Images.Add(new ProductImageEntity { ImageUrl = realGalleryPath }); // add new image to product entity
                }
            }
        }
        private string GetProductFolderId(ProductEntity product)
        {
            if (string.IsNullOrEmpty(product.MainImageUrl))
                return Guid.NewGuid().ToString(); // Generate new folder if no main image
            var segments = product.MainImageUrl.Split('/');
            int productsIndex = Array.IndexOf(segments, "Products");
            if (productsIndex >= 0 && productsIndex < segments.Length - 1)
                return segments[productsIndex + 1]; // Return existing folder name
            return Guid.NewGuid().ToString(); // Fallback to new folder if structure is unexpected
        }
        #endregion
    }
}
