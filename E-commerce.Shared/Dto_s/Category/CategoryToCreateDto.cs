using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace E_commerce.Shared.Dto_s.Category
{
    public class CategoryToCreateDto
    {
        [Required(ErrorMessage = "Category Title Required")]
        [MaxLength(100, ErrorMessage = "Max Length 100")]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string PictureUrl { get; set; } = string.Empty;
    }
}
