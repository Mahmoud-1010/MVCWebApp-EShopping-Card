using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;

namespace PresentationLayer.Models
{
    public class OrderViewModel
    {
       
        public OrderHeader OrderHeader { get; set; }
        public IEnumerable<OrderDetails> OrderDetails { get; set; }
    }
}
