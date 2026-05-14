using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Shared.Dto_s.Address
{
    public class AddressDto
    {
         public int Id { get; set; }
        public string City { get; set; } = string.Empty;
        public string StreetDetails { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
    }
}
