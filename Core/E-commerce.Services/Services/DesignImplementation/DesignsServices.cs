using AutoMapper;
using E_commerce.Abstraction.IService.Attachment;
using E_commerce.Abstraction.IService.Designs;
using E_commerce.Domain.Contracts.UnitOfWorkPattern;
using E_commerce.Domain.Models.Designs;
using E_commerce.Services.Specification.Design;
using E_commerce.Shared.Dto_s.Design;
using E_commerce.Shared.EnumsHelper.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Services.DesignImplementation
{
    public class DesignsServices(IUnitOfWork _unitOfWork, 
        IMapper _mapper,
        IAttachmentService _attachmentService) : IDesignsService
    {
        public async Task<IReadOnlyList<DesignDto>> GetAllDesignsAsync(DesignGender? gender)
        {
            var spec = new DesignByGenderFiltrationSpec(gender);

            var designs = await _unitOfWork.GetRepository<DesignsEntity, int>().GetAllWithSpecAsync(spec);

            return _mapper.Map<IReadOnlyList<DesignDto>>(designs);
        }

        public async Task<DesignDto> AddDesignAsync(DesignToCreateDto dto)
        {
            var imageUrl = await _attachmentService.UploadImageAsync(dto.Image, "designs");

            var designEntity = _mapper.Map<DesignsEntity>(dto);

            designEntity.ImageUrl = imageUrl;

            await _unitOfWork.GetRepository<DesignsEntity, int>().AddAsync(designEntity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<DesignDto>(designEntity);
        }

        public async Task<bool> DeleteDesignAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<DesignsEntity, int>();

            var designEntity = await repo.GetByIdAsync(id);
            if (designEntity == null) return false;

            // 1. مسح الديزاين نهائياً من الداتابيز
            repo.Delete(designEntity);
            var result = await _unitOfWork.SaveChangesAsync();

            // 🚨 تنبيه هام جداً بخصوص ملف الصورة:
            // لا تقم باستدعاء _attachmentService.DeleteImage(designEntity.ImageUrl) هنا!
            // اترك ملف الصورة موجوداً في مجلد uploads على السيرفر، لأن الفواتير القديمة
            // لا تزال تحتاج هذا الرابط لعرض صورة الديزاين للعميل والأدمن في الـ History.

            return result > 0;
        }
    }
}
