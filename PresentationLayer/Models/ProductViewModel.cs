using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PresentationLayer.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]// Ensures non-negative price
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        
        [ValidateNever]
        public string ImageUrl { get; set; }
        public IFormFile Image { get; set; }
        public int CategoryId { get; set; }

    }
}
