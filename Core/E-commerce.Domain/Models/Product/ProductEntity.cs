using E_commerce.Domain.Models.Category;
using E_commerce.Domain.Models.CustomerInteraction;

namespace E_commerce.Domain.Models.Product
{
    public class ProductEntity : BaseEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string MainImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }

        // t-shirt-tom color : red size - xl stock : 10
        // t-shirt-tom color : red size - l stock : 10
        // t-shirt-tom color : blue size - l stock : 9
        // t-shirt-tom color : green size - l stock : 9
        // t-shirt-tom color : green size - xl stock : 9
        // t-shirt-tom color : green size - m stock : 9
        // t-shirt-tom color : green size - s stock : 9
        //  ---------------------- 

        // product id = 1[name , desc , main image , price , category ] ,
        // product variant id = 10 [quantity =10 , color=red , size=xl]
        // product variant id = 11 [quantity =10 , color=blue , size=xl]
        // product id = 1 , product variant id = 10 
        // product id = 1 , product variant id = 11 

        // Foreign Key 
        public int CategoryId { get; set; }
        public virtual CategoryEntity Category { get; set; } = null!;

        // Navigation Properties
        public virtual ICollection<ProductImageEntity> Images { get; set; } = new List<ProductImageEntity>();
        public virtual ICollection<ProductVariantEntity> Variants { get; set; } = new List<ProductVariantEntity>();
        public virtual ICollection<ProductReviewEntity> Reviews { get; set; } = new List<ProductReviewEntity>();
    }
}
