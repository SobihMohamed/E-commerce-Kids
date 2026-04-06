using E_commerce.Domain.Contracts.Specifications.BaseSpec;
using E_commerce.Domain.Models.CustomerInteraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Specification.ShoppingCart
{
    public class GetShoppingCartByUserIdSpec : BaseSpecifications<ShoppingCartEntity, Guid>
    {
        public GetShoppingCartByUserIdSpec(string userId) 
            : base(c => c.UserId == userId)
        {
            AddInclude(c => c.CartItems);

            var variantInclude = $"{nameof(ShoppingCartEntity.CartItems)}.{nameof(CartItemEntity.ProductVariant)}";
            IncludeStrings.Add(variantInclude);
        }
    }
}
