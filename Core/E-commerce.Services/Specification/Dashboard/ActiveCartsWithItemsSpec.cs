using E_commerce.Domain.Contracts.Specifications.BaseSpec;
using E_commerce.Domain.Models.CustomerInteraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Specification.Dashboard
{
    public class ActiveCartsWithItemsSpec : BaseSpecifications<ShoppingCartEntity, Guid>
    {
        public ActiveCartsWithItemsSpec()
            : base(c => c.CartItems.Any())
        {
        }
    }
}
