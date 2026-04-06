using E_commerce.Domain.Contracts.Specifications.BaseSpec;
using E_commerce.Domain.Models.CustomerInteraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Specification.ShoppingCart
{
    public class GetShoppingCartByUserId : BaseSpecifications<ShoppingCartEntity, Guid>
    {
        public GetShoppingCartByUserId(string userId) 
            : base(c => c.UserId == userId)
        {
            
        }
    }
}
