using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Domain.DbInitializer
{
    public interface IDbInitializer
    {
        Task DataSeedAsync();
    }
}
