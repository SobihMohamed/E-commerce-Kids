using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace E_commerce.Shared.Dto_s.Lookups.Color
{
    public class ColorToUpdateDto
    {
        [MaxLength(50)]
        public string? Name { get; set; } 

        [MaxLength(20)]
        public string? HexCode { get; set; } 
    }
}
