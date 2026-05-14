using E_commerce.Domain.Contracts.Specifications.BaseSpec;
using E_commerce.Domain.Models.Address;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Specification.Address
{
    public class GetAddressesForUserSpec : BaseSpecifications<AddressEntity, int>
    {
        public GetAddressesForUserSpec(string userId)
            : base(a => a.UserId == userId)
        {
        }
    }
}
