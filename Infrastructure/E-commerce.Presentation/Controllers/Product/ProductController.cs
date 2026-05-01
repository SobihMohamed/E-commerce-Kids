using E_commerce.Abstraction.IService.Product;
using E_commerce.Shared.Common.Pagination;
using E_commerce.Shared.Common.Params.Product;
using E_commerce.Shared.Common.Responses; 
using E_commerce.Shared.Dto_s.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Presentation.Controllers.Product
{
    public class ProductController : AppBaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // 1. GET ALL PRODUCTS WITH FILTERING, SORTING, PAGINATION
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PaginationResponse<ProductDto>>), 200)]
        public async Task<ActionResult> GetAllProducts([FromQuery] ProductSpecParams specParams)
        {
            var products = await _productService.GetAllProductsAsync(specParams);
            return Success(products, "Products get successfully");
        }

        // 2. get product details by id
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ProductDetailsDto>), 200)]
        public async Task<ActionResult> GetProductDetails(int id)
        {
            var product = await _productService.GetProductDetailsByIdAsync(id);
            return Success(product, "product details get successfully");
        }

        // 3. create product
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ProductDetailsDto>), 201)]
        public async Task<ActionResult> CreateProduct([FromForm] ProductToCreateDto dto)
        {
            var product = await _productService.CreateProductAsync(dto);
            return Created(product, "Prodcut Created Successfully");
        }

        // 4. update product
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ProductDetailsDto>), 200)]
        public async Task<ActionResult> UpdateProduct(int id, [FromForm] ProductToUpdateDto dto)
        {
            var product = await _productService.UpdateProductAsync(id, dto);
            return Success(product, "Product Updated Successfully");
        }

        // 5. delete product
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);

            if (!result)
                return BadRequestError("Error While Deleting the product");

            return Success("Product Deleted successfully");
        }
    }
}