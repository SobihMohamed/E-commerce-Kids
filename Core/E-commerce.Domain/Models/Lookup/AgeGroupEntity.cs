using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Domain.Models.Lookup
{
    public class AgeGroupEntity : BaseEntity<int>
    {
        public string Name { get; set; } = string.Empty; // "4-6"
    }
}
