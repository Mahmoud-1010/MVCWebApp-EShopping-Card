using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models
{
    public class CateegoryViewModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
