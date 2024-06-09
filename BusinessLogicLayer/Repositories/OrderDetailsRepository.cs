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
    public class OrderDetailsRepository:GenericRepository<OrderDetails>,IOrderDetailsRepository
    {
        public ApplicationDBContext _context { get; }

        public OrderDetailsRepository(ApplicationDBContext context):base(context) 
        {
            _context = context;
        }

    }
}
