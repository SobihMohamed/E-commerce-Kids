using E_commerce.Abstraction.IService.Shipping;
using E_commerce.Shared.Dto_s.Shipping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Presentation.Controllers.Shipping
{
    [Route("api/shipping-rates")]
    public class ShippingRatesController : AppBaseController
    {
        private readonly IShippingRateService _shippingRateService;

        public ShippingRatesController(IShippingRateService shippingRateService)
        {
            _shippingRateService = shippingRateService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ShippingRateDto>>> GetAllRates()
        {
            var rates = await _shippingRateService.GetAllRatesAsync();

            return Success(rates, "تم استرجاع أسعار الشحن بنجاح");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ShippingRateDto>> UpdateRate(int id, [FromBody] UpdateShippingRateDto dto)
        {
            var updatedRate = await _shippingRateService.UpdateRateAsync(id, dto);

            return Success(updatedRate, "تم تحديث سعر الشحن بنجاح");
        }
    }
}