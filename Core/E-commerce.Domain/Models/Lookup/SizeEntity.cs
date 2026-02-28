using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Domain.Models.Lookup
{
    public class SizeEntity : BaseEntity<int>
    {
        public string Name { get; set; } = string.Empty; // "S", "M", "L", "4", "6", "8"
    }
}
