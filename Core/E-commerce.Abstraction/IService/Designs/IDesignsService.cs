using E_commerce.Shared.Dto_s.Design;
using E_commerce.Shared.EnumsHelper.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Abstraction.IService.Designs
{
    public interface IDesignsService
    {
        Task<IReadOnlyList<DesignDto>> GetAllDesignsAsync(DesignGender? gender);
        Task<DesignDto> AddDesignAsync(DesignToCreateDto dto);
        Task<bool> DeleteDesignAsync(int id);
    }
}
