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
        public async Task<ProductDetailsDto> GetProductDetailsByIdAsync(int id)
        {
            var productRepo = _unitOfWork.GetRepository<ProductEntity, int>();

            // 1.create spec to get the product with all related data (category, images, variants, reviews)
            var spec = new ProductByIdSpec(id);

            // 2 get the product using the spec
            var product = await productRepo.GetByIdWithSpecAsync(spec);
            if (product == null)
                throw new NotFoundExceptionCustome($"Product with ID {id} not found.");

            //Mapping
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
        public Task<ProductDetailsDto> UpdateProductAsync(int id, ProductToUpdateDto productDto)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }


    }
}
