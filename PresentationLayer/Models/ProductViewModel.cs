using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PresentationLayer.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [Range(0, double.MaxValue)] // Ensures non-negative price
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [DisplayName("Image")]
        public string ImageUrl { get; set; }
       
        public int CategoryId { get; set; }

    }
}
