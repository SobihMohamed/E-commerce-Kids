using E_commerce.Abstraction.IService.Product;
using E_commerce.Shared.Common.Pagination;
using E_commerce.Shared.Common.Params.Product;
using E_commerce.Shared.Common.Responses; 
using E_commerce.Shared.Dto_s.Product;
using E_commerce.Shared.EnumsHelper.Product;
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
            specParams.IsBaseGarment = false;
            specParams.IsActive = true;
            var products = await _productService.GetAllProductsAsync(specParams);
            return Success(products, "Products retrieved successfully");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/all")]
        public async Task<ActionResult> GetAllProductsForAdmin([FromQuery] ProductSpecParams specParams)
        {
            specParams.IsActive = null; // Admin can see both active and inactive products
            var products = await _productService.GetAllProductsAsync(specParams);
            return Success(products, "Products retrieved successfully");
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ProductDetailsDto>), 200)]
        public async Task<ActionResult> GetProductDetails(int id)
        {
            var product = await _productService.GetProductDetailsByIdAsync(id, isBaseGarment: false);
            return Success(product, "Product details retrieved successfully");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/{id}")]
        [ProducesResponseType(typeof(ApiResponse<ProductDetailsDto>), 200)]
        public async Task<ActionResult> GetProductDetailsForAdmin(int id)
        {
            var product = await _productService.GetProductDetailsByIdAsync(id);
            return Success(product, "Product details retrieved successfully");
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

        [HttpGet("customization-product/{gender}")]
        [ProducesResponseType(typeof(ApiResponse<ProductDetailsDto>), 200)]
        public async Task<ActionResult> GetCustomizationProductAsync(TargetGender gender)
        {
            var product = await _productService.GetCustomizationProductAsync(gender);
            return Success(product, "Customization base garment retrieved successfully.");
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

        // 6. toggle product activity status (Active / Inactive)
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/toggle-status")]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<ActionResult> ToggleProductStatus(int id)
        {
            var result = await _productService.ToggleProductActivityAsync(id);

            if (!result)
                return BadRequestError("Error while toggling product status. Product may not exist.");

            return Success("Product status toggled successfully.");
        }
    }
}