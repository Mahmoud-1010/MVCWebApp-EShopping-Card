﻿using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models
{
    public class CateegoryViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
