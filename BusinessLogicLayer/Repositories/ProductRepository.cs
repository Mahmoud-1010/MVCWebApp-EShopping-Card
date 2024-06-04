using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ApplicationDBContext _context { get; }

        public ProductRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> Search(string Name)
            => await _context.Products.Where(P => P.Name.Contains(Name)).ToListAsync();
    }
}
