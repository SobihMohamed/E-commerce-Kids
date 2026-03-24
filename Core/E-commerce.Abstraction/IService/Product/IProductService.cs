using E_commerce.Shared.Dto_s.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Abstraction.IService.Product
{
    public interface IProductService
    {
        Task<IReadOnlyList<ProductDto>> GetAllProductsAsync();
        Task<ProductDetailsDto> GetProductDetailsByIdAsync(int id);

         Task<ProductDetailsDto> CreateProductAsync(ProductToCreateDto productDto);
         Task<ProductDetailsDto> UpdateProductAsync(int id, ProductToUpdateDto productDto);
         Task<bool> DeleteProductAsync(int id);
    }
}
