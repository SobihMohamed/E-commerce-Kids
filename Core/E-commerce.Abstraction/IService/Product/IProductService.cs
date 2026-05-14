using E_commerce.Shared.Common.Pagination;
using E_commerce.Shared.Common.Params.Product;
using E_commerce.Shared.Dto_s.Product;
using E_commerce.Shared.EnumsHelper.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Abstraction.IService.Product
{
    public interface IProductService
    {
        Task<PaginationResponse<ProductDto>> GetAllProductsAsync(ProductSpecParams specParams);
        Task<ProductDetailsDto> GetProductDetailsByIdAsync(int id, bool? isBaseGarment = null);
        Task<ProductDetailsDto> CreateProductAsync(ProductToCreateDto productDto);
        Task<ProductDetailsDto> UpdateProductAsync(int id, ProductToUpdateDto productDto);
        Task<ProductDetailsDto> GetCustomizationProductAsync(TargetGender gender);
        Task<bool> DeleteProductAsync(int id);
        Task<bool> ToggleProductActivityAsync(int id);
    }
}
