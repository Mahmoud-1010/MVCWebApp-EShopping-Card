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
    public class ShoppingCartRepository : GenericRepository<ShoppingCart>,IShoppingCartRepository
    {
        public ApplicationDBContext _context { get; }

        public ShoppingCartRepository(ApplicationDBContext Context):base(Context)
        {
            _context = Context;
        }
    }
}
