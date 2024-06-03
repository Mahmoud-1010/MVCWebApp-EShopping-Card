using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [Range(0, double.MaxValue)] // Ensures non-negative price
        public decimal Price { get; set; }
        //[DisplayName("Select Image")]
        //public string ImageUrl { get; set; }
        [ForeignKey("Category")]
        public int? CategoryId { get; set; } // Foreign key for Category relationship
        public virtual Category Category { get; set; } // Navigation property for Category
    }
}
