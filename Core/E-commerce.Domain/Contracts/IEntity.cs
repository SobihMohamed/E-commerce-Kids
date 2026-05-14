using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Domain.Contracts
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
