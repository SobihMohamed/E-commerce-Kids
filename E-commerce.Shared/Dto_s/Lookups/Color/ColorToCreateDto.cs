using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace E_commerce.Shared.Dto_s.Lookups.Color
{
    public class ColorToCreateDto
    {
        [Required(ErrorMessage = "Color Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hex Code is required")]
        public string HexCode { get; set; } = string.Empty;
    }
}
