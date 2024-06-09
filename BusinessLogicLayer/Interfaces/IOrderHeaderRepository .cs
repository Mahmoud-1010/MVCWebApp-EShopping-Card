using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IOrderHeaderRepository:IGenericRepository<OrderHeader>
    {
        void UpdateOrderStatus(int id, string OrderStatus, string PaymentStatus);
    }
}
