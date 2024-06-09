﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public virtual OrderHeader OrderHeader { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
