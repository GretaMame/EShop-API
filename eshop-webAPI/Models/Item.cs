using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eshopAPI.Models
{
    public class Item
    {
        [Key]
        public long ID { get; set; } // Primary key
        [Required]
        [MaxLength(10)]
        public string SKU { get; set; } // Bussiness key
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }
        [Required]
        [MaxLength(5000)]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreateDate { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ModifiedDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey("SubCategory")]
        public long SubCategoryID { get; set; }
        public SubCategory SubCategory { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }

        public virtual ICollection<AttributeValue> Attributes { get; set; }
        public virtual ICollection<ItemPicture> Pictures { get; set; }
    }
}
