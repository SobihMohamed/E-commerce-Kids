using AutoMapper;
using E_commerce.Abstraction.IService.Lookup;
using E_commerce.Domain.Contracts.UnitOfWorkPattern;
using E_commerce.Domain.Exceptions; 
using E_commerce.Domain.Models.Lookup;
using E_commerce.Shared.Dto_s.Lookups.Color;
using E_commerce.Shared.Dto_s.Lookups.Size;

namespace E_commerce.Services.Services.LookupImplementation
{
    public class LookUpService(IUnitOfWork unitOfWork, IMapper mapper) : ILookupService
    {
        // ==========================================================
        // 1. Colors Methods
        // ==========================================================
        public async Task<IReadOnlyList<ColorDto>> GetAllColorsAsync()
        {
            var colors = await unitOfWork.GetRepository<ColorEntity, int>().GetAllAsync();
            return mapper.Map<IReadOnlyList<ColorDto>>(colors);
        }

        public async Task<ColorDto> AddColorAsync(ColorToCreateDto colorToCreate)
        {
            var colorEntity = mapper.Map<ColorEntity>(colorToCreate);

            await unitOfWork.GetRepository<ColorEntity, int>().AddAsync(colorEntity);
            await unitOfWork.SaveChangesAsync();

            return mapper.Map<ColorDto>(colorEntity);
        }

        public async Task DeleteColorAsync(int id)
        {
            var colorEntity = await unitOfWork.GetRepository<ColorEntity, int>().GetByIdAsync(id);
            if (colorEntity != null)
            {
                unitOfWork.GetRepository<ColorEntity, int>().Delete(colorEntity);

                try
                {
                    await unitOfWork.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new BadRequestExceptionCustome(
                        "لا يمكن حذف هذا اللون لأنه مستخدم بالفعل في بعض المنتجات المتاحة بالنظام.");
                }
            }
        }

        // ==========================================================
        // 2. Sizes Methods
        // ==========================================================
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

                try
                {
                    await unitOfWork.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new BadRequestExceptionCustome(
                        "لا يمكن حذف هذا المقاس لأنه مستخدم بالفعل في بعض المنتجات المتاحة بالنظام.");
                }
            }
        }
    }
}