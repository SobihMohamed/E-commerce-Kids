using E_commerce.Abstraction.IService.Address;
using E_commerce.Shared.Common.Responses;
using E_commerce.Shared.Dto_s.Address;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace E_commerce.Presentation.Controllers.Address
{
    [Authorize]
    public class AddressController(IAddressService addressService) : AppBaseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AddressDto>>), 200)]
        public async Task<ActionResult> GetUserAddresses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var addresses = await addressService.GetUserAddressesAsync(userId!);

            return Success(addresses);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<AddressDto>), 200)]
        public async Task<ActionResult> GetAddressById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var address = await addressService.GetAddressByIdAsync(userId!, id);

            return Success(address);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<AddressDto>), 201)] 
        public async Task<ActionResult> AddAddress([FromBody] AddressToSaveDto addressDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var createdAddress = await addressService.AddAddressAsync(userId!, addressDto);

            return Created(createdAddress, "Address created successfully");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<AddressDto>), 200)]
        public async Task<ActionResult> UpdateAddress(int id, [FromBody] AddressToSaveDto addressDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var updatedAddress = await addressService.UpdateAddressAsync(userId!, id, addressDto);

            return Success(updatedAddress, "Address updated successfully");
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)] 
        public async Task<ActionResult> DeleteAddress(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await addressService.DeleteAddressAsync(userId!, id);

            return Success("Address deleted successfully");
        }

        [HttpPatch("{id}/set-default")]
        [ProducesResponseType(typeof(ApiResponse<AddressDto>), 200)]
        public async Task<ActionResult> SetDefaultAddress(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var defaultAddress = await addressService.SetDefaultAddressAsync(userId!, id);

            return Success(defaultAddress, "Default address updated successfully");
        }
    }
}