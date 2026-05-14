using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Shared.EnumsHelper.Order
{
    public enum OrderStatus
    {
        Pending = 1,      
        Processing = 2,   
        Shipped = 3,      
        Delivered = 4,    
        Cancelled = 5    
    }
}
