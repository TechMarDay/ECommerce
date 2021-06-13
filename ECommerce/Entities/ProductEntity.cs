using System.Collections.Generic;
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

        public int? AttachmentId { get; set; }

        public string Image { get; set; }

        [Column(TypeName = "decimal(18, 3)")]
        public decimal Price { get; set; }

        public int? Discount { get; set; }

        [Column(TypeName = "decimal(18, 3)")]
        public decimal? DiscountPrice { get; set; }

        public bool? BestSeller { get; set; }

        public string Url { get; set; }

        public virtual ProductCategoryEntity Category { get; set; }

        public virtual ICollection<AttachmentEntity> Attachments { get; set; }
    }   
}