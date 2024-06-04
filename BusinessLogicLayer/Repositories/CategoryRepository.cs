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
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public ApplicationDBContext _context { get; }

        public CategoryRepository(ApplicationDBContext context):base(context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> Search(string Name)
            => await _context.Categories.Where(C => C.Name.Contains(Name)).ToListAsync();


    }
}
