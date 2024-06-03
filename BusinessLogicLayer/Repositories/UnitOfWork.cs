using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public ApplicationDBContext _context { get; }
        public ICategoryRepository Category { get; private set; }

        public UnitOfWork(ApplicationDBContext context)
        {
            _context = context;
            Category =new CategoryRepository(context);
        }


        public int Complete()
        {
           return _context.SaveChanges();
        }

        public void Dispose()
        {
             _context.Dispose();
        }
    }
}
