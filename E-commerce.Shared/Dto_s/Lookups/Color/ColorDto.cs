using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Shared.Dto_s.Lookups.Color
{
    public class ColorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; 
        public string HexCode { get; set; } = string.Empty; 
    }
}
