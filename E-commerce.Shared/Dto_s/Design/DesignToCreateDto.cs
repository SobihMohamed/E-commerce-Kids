using E_commerce.Shared.EnumsHelper.Design;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace E_commerce.Shared.Dto_s.Design
{
    public class DesignToCreateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public IFormFile Image { get; set; } // to upload the image file

        [Required]
        public DesignGender DesignGender { get; set; }
    }
}
