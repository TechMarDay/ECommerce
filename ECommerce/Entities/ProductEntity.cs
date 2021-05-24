using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Entities
{
    public class ProductEntity : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? CategoryId { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public int? Discount { get; set; }

        public decimal? DiscountPrice { get; set; }

        public bool? BestSeller { get; set; }

        public virtual ProductCategoryEntity Category { get; set; }
    }
}