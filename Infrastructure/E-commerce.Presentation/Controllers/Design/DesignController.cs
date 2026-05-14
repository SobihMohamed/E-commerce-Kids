using E_commerce.Abstraction.IService.Designs;
using E_commerce.Shared.Common.Responses; 
using E_commerce.Shared.Dto_s.Design;
using E_commerce.Shared.EnumsHelper.Design;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Presentation.Controllers.Design
{
    public class DesignController : AppBaseController
    {
        private readonly IDesignsService _designsService;

        public DesignController(IDesignsService designsService)
        {
            _designsService = designsService;
        }

        // 1. GET ALL DESIGNS (With Optional Filter)
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<DesignDto>>), 200)]
        public async Task<ActionResult> GetAllDesigns([FromQuery] DesignGender? gender)
        {
            var designs = await _designsService.GetAllDesignsAsync(gender);
            return Success(designs, "Designs retrieved successfully");
        }

        // 2. CREATE NEW DESIGN (Uploads Image)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<DesignDto>), 201)] 
        public async Task<ActionResult> AddDesign([FromForm] DesignToCreateDto dto)
        {
            var design = await _designsService.AddDesignAsync(dto);
            return Created(design, "Design added and image uploaded successfully");
        }

        // 3. DELETE DESIGN (Removes Image + DB Record)
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)] 
        [ProducesResponseType(typeof(ApiResponse<object>), 400)] 
        public async Task<ActionResult> DeleteDesign(int id)
        {
            var result = await _designsService.DeleteDesignAsync(id);

            if (!result)
                return BadRequestError("Error while deleting the design, or design not found");

            return Success("Design and its associated image deleted successfully");
        }
    }
}