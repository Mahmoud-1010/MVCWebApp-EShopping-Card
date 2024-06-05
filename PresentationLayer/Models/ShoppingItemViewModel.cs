using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresentationLayer.Models
{
    public class ShoppingItemViewModel
    {
        [StringLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [Range(0, double.MaxValue)] // Ensures non-negative price
        public decimal Price { get; set; }
        //[DisplayName("Select Image")]
        public string ImageUrl { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        [Range(1, 100,ErrorMessage ="You Can't buy more tham 100 Item")]
        public int Count { get; set; }
    }
}
