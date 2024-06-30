using DataAccessLayer.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresentationLayer.Models
{
    public class OrderHeaderViewModel
    {
        public int Id { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string? Phone { get; set; }
    }
}
