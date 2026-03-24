using AutoMapper;
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
    public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
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
        public Task<ProductDetailsDto> CreateProductAsync(ProductToCreateDto productDto)
        {
            throw new NotImplementedException();
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
