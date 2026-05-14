using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace E_commerce.Shared.Dto_s.Lookups.Size
{
    public class SizeToCreateDto
    {
        [Required(ErrorMessage = "Size Name is required")]
        public string Name { get; set; } = string.Empty;
    }
}
