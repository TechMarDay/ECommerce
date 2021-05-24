using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ECommerce.Models.ProductCategoryEnum;

namespace ECommerce.Entities
{
    public class ProductCategoryEntity : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }

        public ProductCategoryType Type { get; set; }

        public virtual ICollection<ProductEntity> Products { get; set; }
    }
}