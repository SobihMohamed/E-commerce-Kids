using AutoMapper;
using E_commerce.Abstraction.IService.Lookup;
using E_commerce.Domain.Contracts.UnitOfWorkPattern;
using E_commerce.Domain.Models.Lookup;
using E_commerce.Shared.Dto_s.Lookups.Color;
using E_commerce.Shared.Dto_s.Lookups.Size;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Services.LookupImplementation
{
    public class LookUpService(IUnitOfWork unitOfWork ,IMapper mapper) : ILookupService
    {
        // ==========================================
        // 1. Colors Methods
        // ==========================================
        public async Task<IReadOnlyList<ColorDto>> GetAllColorsAsync()
        {
            var colors = await unitOfWork.GetRepository<ColorEntity, int>().GetAllAsync();
            return mapper.Map<IReadOnlyList<ColorDto>>(colors);
        }

        public async Task<ColorDto> AddColorAsync(ColorToCreateDto colorToCreate)
        {
            // Map DTO to Entity
            var colorEntity = mapper.Map<ColorEntity>(colorToCreate);

            // Add to Database
            await unitOfWork.GetRepository<ColorEntity, int>().AddAsync(colorEntity);
            await unitOfWork.SaveChangesAsync(); // Save Changes

            // Return the newly created entity mapped back to Dto (Now it has an ID)
            return mapper.Map<ColorDto>(colorEntity);
        }

        public async Task DeleteColorAsync(int id)
        {
            var colorEntity = await unitOfWork.GetRepository<ColorEntity, int>().GetByIdAsync(id);
            if (colorEntity != null)
            {
                unitOfWork.GetRepository<ColorEntity, int>().Delete(colorEntity);
                await unitOfWork.SaveChangesAsync();
            }
        }

        // ==========================================
        // 2. Sizes Methods
        // ==========================================
        public async Task<IReadOnlyList<SizeDto>> GetAllSizesAsync()
        {
            var sizes = await unitOfWork.GetRepository<SizeEntity, int>().GetAllAsync();
            return mapper.Map<IReadOnlyList<SizeDto>>(sizes);
        }

        public async Task<SizeDto> AddSizeAsync(SizeToCreateDto sizeToCreate)
        {
            var sizeEntity = mapper.Map<SizeEntity>(sizeToCreate);

            await unitOfWork.GetRepository<SizeEntity, int>().AddAsync(sizeEntity);
            await unitOfWork.SaveChangesAsync();

            return mapper.Map<SizeDto>(sizeEntity);
        }

        public async Task DeleteSizeAsync(int id)
        {
            var sizeEntity = await unitOfWork.GetRepository<SizeEntity, int>().GetByIdAsync(id);
            if (sizeEntity != null)
            {
                unitOfWork.GetRepository<SizeEntity, int>().Delete(sizeEntity);
                await unitOfWork.SaveChangesAsync();
            }
        }
    }
}
