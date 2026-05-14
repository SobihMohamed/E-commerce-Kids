using AutoMapper;
using E_commerce.Abstraction.IService.Address;
using E_commerce.Domain.Contracts.GenericReposPattern;
using E_commerce.Domain.Contracts.UnitOfWorkPattern;
using E_commerce.Domain.Exceptions;
using E_commerce.Domain.Models.Address;
using E_commerce.Services.Specification.Address;
using E_commerce.Shared.Dto_s.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E_commerce.Services.Services.AddressImplementaion
{
    public class AddressService(IUnitOfWork unitOfWork, IMapper mapper) : IAddressService
    {
        public async Task<AddressDto> AddAddressAsync(string userId, AddressToSaveDto addressDto)
        {
            var addressRepo = GetAddressRepository();
            var userAddresses = await addressRepo.GetAllWithSpecAsync(new GetAddressesForUserSpec(userId));

            var addressEntity = mapper.Map<AddressEntity>(addressDto);
            addressEntity.UserId = userId;

            if (!userAddresses.Any())
            {
                addressEntity.IsDefault = true;
            }
            else if (addressDto.IsDefault)
            {
                await MakeOtherAddressesNonDefaultAsync(userId);
            }

            await addressRepo.AddAsync(addressEntity);

            var result = await unitOfWork.SaveChangesAsync();
            if (result <= 0)
                throw new BadRequestExceptionCustome("Failed to add address");

            return mapper.Map<AddressDto>(addressEntity);
        }

        public async Task DeleteAddressAsync(string userId, int addressId)
        {
            var addressRepo = GetAddressRepository();
            var address = await GetAddressForUserOrThrowAsync(userId, addressId);
            if(address.IsDefault)
                throw new BadRequestExceptionCustome("Cannot delete default address. Please set another address as default before deleting this one.");

            addressRepo.Delete(address);

            var result = await unitOfWork.SaveChangesAsync();
            if (result <= 0)
                throw new BadRequestExceptionCustome("Failed to delete address");
        }

        public async Task<AddressDto> GetAddressByIdAsync(string userId, int addressId)
        {
            var address = await GetAddressForUserOrThrowAsync(userId, addressId);
            return mapper.Map<AddressDto>(address);
        }

        public async Task<IReadOnlyList<AddressDto>> GetUserAddressesAsync(string userId)
        {
            var addressRepo = GetAddressRepository();
            var userAddresses = await addressRepo.GetAllWithSpecAsync(new GetAddressesForUserSpec(userId));

            return mapper.Map<IReadOnlyList<AddressDto>>(userAddresses);
        }

        public async Task<AddressDto> SetDefaultAddressAsync(string userId, int addressId)
        {
            var addressRepo = GetAddressRepository();
            var address = await GetAddressForUserOrThrowAsync(userId, addressId);

            await MakeOtherAddressesNonDefaultAsync(userId, address.Id);

            address.IsDefault = true;
            addressRepo.Update(address);

            await unitOfWork.SaveChangesAsync();

            return mapper.Map<AddressDto>(address);
        }

        public async Task<AddressDto> UpdateAddressAsync(string userId, int addressId, AddressToSaveDto addressDto)
        {
            var addressRepo = GetAddressRepository();
            var address = await GetAddressForUserOrThrowAsync(userId, addressId);
            if(address.IsDefault && !addressDto.IsDefault)
                throw new BadRequestExceptionCustome("Cannot unset default address. Please set another address as default before unsetting this one.");
           
            mapper.Map(addressDto, address);

            if (addressDto.IsDefault)
            {
                await MakeOtherAddressesNonDefaultAsync(userId, address.Id);
            }

            addressRepo.Update(address);

            var result = await unitOfWork.SaveChangesAsync();
            if (result <= 0)
                throw new BadRequestExceptionCustome("Failed to update address");

            return mapper.Map<AddressDto>(address);
        }

        private IGenericRepo<AddressEntity, int> GetAddressRepository()
            => unitOfWork.GetRepository<AddressEntity, int>();

        private async Task<AddressEntity> GetAddressForUserOrThrowAsync(string userId, int addressId)
        {
            var addressRepo = GetAddressRepository();
            var address = await addressRepo.GetByIdWithSpecAsync(new GetAddressByIdForUserSpec(userId, addressId));

            if (address == null)
                throw new NotFoundExceptionCustome("Address not found");

            return address;
        }

        private async Task MakeOtherAddressesNonDefaultAsync(string userId, int? currentAddressId = null)
        {
            var addressRepo = GetAddressRepository();
            var userAddresses = await addressRepo.GetAllWithSpecAsync(new GetAddressesForUserSpec(userId));

            var addressesToUpdate = userAddresses.Where(a => a.IsDefault && (!currentAddressId.HasValue || a.Id != currentAddressId.Value));

            foreach (var userAddress in addressesToUpdate)
            {
                userAddress.IsDefault = false;
                addressRepo.Update(userAddress);
            }
        }
    }
}
