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
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public ApplicationDBContext _context { get; }

        public CategoryRepository(ApplicationDBContext context):base(context) 
        {
            _context = context;
        }


        public async Task<int> UpdateAsync(Category category)
        {
            var categoryInDb= _context.Categories.FirstOrDefault(c=>c.Id==category.Id);
            if (categoryInDb == null)
            {
                categoryInDb.Name = category.Name;
                categoryInDb.Description = category.Description;
                categoryInDb.CreatedDate = DateTime.Now;
            }

            _context.Categories.Update(categoryInDb);
            return await _context.SaveChangesAsync();
        }
    }
}
