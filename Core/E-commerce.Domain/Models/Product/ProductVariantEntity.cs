using E_commerce.Domain.Models.Lookup;


namespace E_commerce.Domain.Models.Product
{
    public class ProductVariantEntity : BaseEntity<int>
    {
        public int StockQuantity { get; set; } 

        // Foreign Keys
        public int ProductId { get; set; }
        public virtual ProductEntity Product { get; set; } = null!;

        public int ColorId { get; set; }
        public virtual ColorEntity Color { get; set; } = null!;

        public int SizeId { get; set; }
        public virtual SizeEntity Size { get; set; } = null!;
    }
}
