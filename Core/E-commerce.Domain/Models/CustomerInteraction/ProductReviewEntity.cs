using E_commerce.Domain.Models.Order;
using E_commerce.Domain.Models.Product;
using E_commerce.Domain.Models.User;

namespace E_commerce.Domain.Models.CustomerInteraction
{
    public class ProductReviewEntity : BaseEntity<int>
    {
        public int Rating { get; set; }

        public string? Comment { get; set; } 

        public bool IsApproved { get; set; } = false;

        // 1. the reviewer must be a registered user
        public string UserId { get; set; } = string.Empty;
        public virtual ApplicationUser User { get; set; } = null!;

        // realted to main product not product variant
        // because we want to show the review on the product details page and not the variant details page
        public int ProductId { get; set; }
        public  virtual ProductEntity Product { get; set; } = null!;

        // related to the order to ensure that only customers who bought the product can review it 
        public Guid OrderId { get; set; }
        public virtual OrderEntity Order { get; set; } = null!;
    }
}
