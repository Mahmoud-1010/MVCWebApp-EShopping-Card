using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public ApplicationDBContext _context { get; }

        public GenericRepository(ApplicationDBContext applicationDBContext)
        {
            _context = applicationDBContext;
        }
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? expression=null, string? IncludeWord=null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (expression != null)
            {
                query = query.Where(expression);
            }
            if (IncludeWord != null)
            {
                foreach (var item in IncludeWord.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return await query.ToListAsync();

        }
        public async Task<T> GetByIdAsync( Expression<Func<T, bool>>? expression = null, string? IncludeWord = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (expression != null)
            {
                query = query.Where(expression);
            }
            if (IncludeWord != null)
            {
                foreach (var item in IncludeWord.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return await query.SingleOrDefaultAsync();
        }
        public async Task<int> InsertAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public Task<int> DeleteByRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Update(T entity)
        {
            //_context.Set<T>().Attach(entity);
            //_context.Entry(entity).State = EntityState.Modified;
            _context.Set<T>().Update(entity);
            return await _context.SaveChangesAsync();

        }
    }
}
