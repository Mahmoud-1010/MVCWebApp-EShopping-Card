using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        //[ForeignKey("Product")]
        public int ProductId { get; set; }
        
        //public virtual Product Product { get; set; }
        public int Count { get; set; }
        //[ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        
        //public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
