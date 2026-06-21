using AutoMapper;
using E_commerce.Abstraction.IService.Shipping;
using E_commerce.Domain.Contracts.UnitOfWorkPattern;
using E_commerce.Domain.Exceptions;
using E_commerce.Domain.Models.Shipping;
using E_commerce.Shared.Dto_s.Shipping;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Services.ShippingImplementation
{
    public class ShippingRateService : IShippingRateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ShippingRateService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ShippingRateDto>> GetAllRatesAsync()
        {
            var repo = _unitOfWork.GetRepository<ShippingRates, int>();
            var rates = await repo.GetAllAsync();
            return _mapper.Map<IReadOnlyList<ShippingRateDto>>(rates);
        }

        public async Task<ShippingRateDto> UpdateRateAsync(int id, UpdateShippingRateDto dto)
        {
            var repo = _unitOfWork.GetRepository<ShippingRates, int>();
            var shippingRate = await repo.GetByIdAsync(id);

            if (shippingRate == null)
                throw new NotFoundExceptionCustome("المحافظة غير موجودة"); 

            shippingRate.Price = dto.NewPrice;

            repo.Update(shippingRate);

            var res = await _unitOfWork.SaveChangesAsync();
            if (res <= 0)
                throw new BadRequestExceptionCustome("حدث خطأ أثناء تحديث سعر الشحن");

            return _mapper.Map<ShippingRateDto>(shippingRate);
        }
    }
}