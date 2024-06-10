using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repositories
{
    public class OrderHeaderRepository : GenericRepository<OrderHeader>, IOrderHeaderRepository
    {
        public ApplicationDBContext _context { get; }

        public OrderHeaderRepository(ApplicationDBContext context):base(context) 
        {
            _context = context;
        }


        public void UpdateOrderStatus(int id, string OrderStatus, string PaymentStatus)
        {
            var orderHeader = _context.OrderHeaders.FirstOrDefault(O => O.Id == id);
            orderHeader.OrderStatus = OrderStatus;
            orderHeader.PaymentStatus = PaymentStatus;
            _context.SaveChanges();
        }
    }
}
