using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Shared.Dto_s.Design
{
    public class DesignDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string DesignGender { get; set; } = string.Empty;
    }
}
