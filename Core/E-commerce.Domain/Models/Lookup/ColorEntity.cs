using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Domain.Models.Lookup
{
    public class ColorEntity : BaseEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public string HexCode { get; set; } = string.Empty; // "#FF0000"
    }
}
