using E_commerce.Shared.Dto_s.Address;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Abstraction.IService.Address
{
    public interface IAddressService
    {
        Task<AddressDto> AddAddressAsync(string userId, AddressToSaveDto addressDto);

        Task<IReadOnlyList<AddressDto>> GetUserAddressesAsync(string userId);

        Task<AddressDto> GetAddressByIdAsync(string userId, int addressId);

        Task<AddressDto> UpdateAddressAsync(string userId, int addressId, AddressToSaveDto addressDto);

        Task DeleteAddressAsync(string userId, int addressId);

        Task<AddressDto> SetDefaultAddressAsync(string userId, int addressId);
    }
}
