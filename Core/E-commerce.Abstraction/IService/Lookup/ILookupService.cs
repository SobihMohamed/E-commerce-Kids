using E_commerce.Shared.Dto_s.Lookups.Color;
using E_commerce.Shared.Dto_s.Lookups.Size;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Abstraction.IService.Lookup
{
    public interface ILookupService
    {
        Task<IReadOnlyList<ColorDto>> GetAllColorsAsync();
        Task<ColorDto> AddColorAsync(ColorToCreateDto colorToCreate);
        Task DeleteColorAsync(int id);

        Task<IReadOnlyList<SizeDto>> GetAllSizesAsync();
        Task<SizeDto> AddSizeAsync(SizeToCreateDto sizeToCreate);
        Task DeleteSizeAsync(int id);
    }
}
