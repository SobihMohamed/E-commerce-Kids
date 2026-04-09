using E_commerce.Domain.Contracts.Specifications.BaseSpec;
using E_commerce.Domain.Models.Address;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Specification.Address
{
    public class GetAddressByIdForUserSpec : BaseSpecifications<AddressEntity, int>
    {
        public GetAddressByIdForUserSpec(string userId, int addressId)
            : base(a => a.UserId == userId && a.Id == addressId)
        {
        }
    }
}
