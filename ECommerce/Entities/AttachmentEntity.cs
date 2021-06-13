using ECommerce.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Entities
{
    public class AttachmentEntity : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Image { get; set; }

        public AttachmentRefEnum.RefId RefId { get; set; }

        public int ProductId { get; set; }

        public virtual ProductEntity Product { get; set; }
    }
}