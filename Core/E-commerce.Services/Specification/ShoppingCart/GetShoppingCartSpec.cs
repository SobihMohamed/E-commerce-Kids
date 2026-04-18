using E_commerce.Domain.Contracts.Specifications.BaseSpec;
using E_commerce.Domain.Models.CustomerInteraction;
using E_commerce.Domain.Models.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Specification.ShoppingCart
{
    public class GetShoppingCartSpec : BaseSpecifications<ShoppingCartEntity, Guid>
    {
        public GetShoppingCartSpec(Guid cartId): base(c => c.Id == cartId)
        {
            // in Ef-Core => .Include(c => c.CartItems)
            AddInclude(c => c.CartItems);

            // include to product Variant in CartItem in EF-Core => .Include(c => c.CartItems).ThenInclude(i => i.ProductVariant) 
            var varientInclude = $"{nameof(ShoppingCartEntity.CartItems)}.{nameof(CartItemEntity.ProductVariant)}";
            IncludeStrings.Add(varientInclude);

            // include to product in Product Variant
            // in EF-Core => .Include(c => c.CartItems).ThenInclude(i => i.ProductVariant).ThenInclude(v => v.Product)
            var productInclude = $"{varientInclude}.{nameof(ProductVariantEntity.Product)}";
            IncludeStrings.Add(productInclude);

            // include to color in Product Variant
            // in EF-Core => .Include(c => c.CartItems).ThenInclude(i => i.ProductVariant).ThenInclude(v => v.Color)
            var colorInclude = $"{varientInclude}.{nameof(ProductVariantEntity.Color)}";
            IncludeStrings.Add(colorInclude);

            // include to size in Product Variant
            // in EF-Core => .Include(c => c.CartItems).ThenInclude(i => i.ProductVariant).ThenInclude(v => v.Size)
            var sizeInclude = $"{varientInclude}.{nameof(ProductVariantEntity.Size)}";
            IncludeStrings.Add(sizeInclude);

            // include to Design in CartItem
            var designInclude = $"{nameof(ShoppingCartEntity.CartItems)}.{nameof(CartItemEntity.Design)}";
            IncludeStrings.Add(designInclude);

            // Order by 
            // ==========================================
            // final query in EF-Core => 
            // _context.ShoppingCarts
            //     .Include(c => c.CartItems)
            //         .ThenInclude(i => i.ProductVariant)
            //             .ThenInclude(v => v.Product)
            //     .Include(c => c.CartItems)
            //         .ThenInclude(i => i.ProductVariant)
            //             .ThenInclude(v => v.Color)
            //     .Include(c => c.CartItems)
            //         .ThenInclude(i => i.ProductVariant)
            //             .ThenInclude(v => v.Size)
            //      .Include(c => c.CartItems)
            //        .ThenInclude(i => i.Design)
            //     .Where(c => c.Id == cartId)
            //     
            // ==========================================

            // ==========================================
            // final query in sql => 
            // SELECT c.*, i.*, v.*, p.*, clr.*, s.* 
            // FROM ShoppingCarts c
            // LEFT JOIN CartItems i ON c.Id = i.ShoppingCartId
            // LEFT JOIN ProductVariants v ON i.ProductVariantId = v.Id
            // LEFT JOIN Products p ON v.ProductId = p.Id
            // LEFT JOIN Colors clr ON v.ColorId = clr.Id
            // LEFT JOIN Sizes s ON v.SizeId = s.Id
            // WHERE c.Id = @cartId
            // ==========================================

        }
    }
}
