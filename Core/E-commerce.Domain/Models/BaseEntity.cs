using E_commerce.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Domain.Models
{
    public class BaseEntity<TKey> : IEntity<TKey>
    {
        public TKey Id { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public string? LastModifiedBy { get; set; } = null!;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
