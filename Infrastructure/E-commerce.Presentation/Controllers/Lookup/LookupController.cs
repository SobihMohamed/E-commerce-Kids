using E_commerce.Abstraction.IService.Lookup;
using E_commerce.Shared.Dto_s.Lookups.Color;
using E_commerce.Shared.Dto_s.Lookups.Size;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Presentation.Controllers.Lookup
{
    public class LookupController : AppBaseController
    {
        private readonly ILookupService _lookupService;

        public LookupController(ILookupService lookupService)
        {
            _lookupService = lookupService;
        }

        [HttpGet("colors")]
        public async Task<ActionResult> GetAllColors()
        {
            var colors = await _lookupService.GetAllColorsAsync();
            return Success(colors, "Colors retrieved successfully");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("colors")]
        public async Task<ActionResult> AddColor([FromBody] ColorToCreateDto dto)
        {
            var color = await _lookupService.AddColorAsync(dto);
            return Created(color, "Color added successfully");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("colors/{id}")]
        public async Task<ActionResult> DeleteColor(int id)
        {
            await _lookupService.DeleteColorAsync(id);
            return Success("Color deleted successfully");
        }

        [HttpGet("sizes")]
        public async Task<ActionResult> GetAllSizes()
        {
            var sizes = await _lookupService.GetAllSizesAsync();
            return Success(sizes, "Sizes retrieved successfully");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("sizes")]
        public async Task<ActionResult> AddSize([FromBody] SizeToCreateDto dto)
        {
            var size = await _lookupService.AddSizeAsync(dto);
            return Created(size, "Size added successfully");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("sizes/{id}")]
        public async Task<ActionResult> DeleteSize(int id)
        {
            await _lookupService.DeleteSizeAsync(id);
            return Success("Size deleted successfully");
        }
    }
}