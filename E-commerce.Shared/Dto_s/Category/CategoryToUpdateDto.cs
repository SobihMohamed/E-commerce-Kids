using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace E_commerce.Shared.Dto_s.Category
{
    public class CategoryToUpdateDto
    {
        [MaxLength(100)]
        public string? Name { get; set; } 

        public string? Description { get; set; }

        public IFormFile? PictureUrl { get; set; }
    }
}
