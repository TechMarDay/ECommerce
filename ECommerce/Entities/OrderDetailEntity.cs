using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Entities
{
    public class OrderDetailEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }
        public int OrderId { set; get; }
        public int ProductId { set; get; }
        public int Quantity { set; get; }

        [Column(TypeName = "decimal(18, 3)")]
        public decimal Price { set; get; }

        public virtual OrderEntity Order { get; set; }

        public virtual ProductEntity Product { get; set; }
    }
}