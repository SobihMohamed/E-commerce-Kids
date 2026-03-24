using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace E_commerce.Shared.Dto_s.Lookups.Size
{
    public class SizeToUpdateDto
    {
        [MaxLength(50)]
        public string? Name { get; set; } 
    }
}
