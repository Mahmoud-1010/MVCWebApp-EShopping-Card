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
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ApplicationDBContext _context { get; }

        public ProductRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
        public async Task<int> UpdateAsync(Product product)
        {
            var productInDb = _context.Products.FirstOrDefault(c => c.Id == product.Id);
            if (productInDb == null)
            {
                productInDb.Name = product.Name;
                productInDb.Description = product.Description;
                productInDb.Price = product.Price;
                productInDb.ImageUrl = product.ImageUrl;
                productInDb.CategoryId = product.CategoryId;
            }

            _context.Products.Update(productInDb);
            return await _context.SaveChangesAsync();
        }
    }
}
