using E_commerce.Shared.EnumsHelper.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Domain.Models.Designs
{
    public class DesignsEntity : BaseEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public DesignGender DesignGender {  get; set; } // this image for boys or girls or both
    }
}
