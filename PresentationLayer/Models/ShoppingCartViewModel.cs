﻿using DataAccessLayer.Entities;

namespace PresentationLayer.Models
{
    public class ShoppingCartViewModel
    {
        public IEnumerable<ShoppingCart> CartsList { get; set; }
    }
}