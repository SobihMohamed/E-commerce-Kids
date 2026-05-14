using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Shared.Dto_s.Category
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty;
        public string PictureUrl { get; set; } = string.Empty; 
    }
}
