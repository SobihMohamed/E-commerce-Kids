using E_commerce.Shared.Dto_s.Shipping;

namespace E_commerce.Abstraction.IService.Shipping
{
    public interface IShippingRateService
    {
        Task<IReadOnlyList<ShippingRateDto>> GetAllRatesAsync();
        Task<ShippingRateDto> UpdateRateAsync(int id, UpdateShippingRateDto dto);
    }
}
