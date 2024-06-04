using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T,bool>>? expression = null, string? IncludeWord=null);
        Task<T> GetByIdAsync(Expression<Func<T, bool>>? expression = null, string? IncludeWord = null);
        Task<int> InsertAsync(T entity);
        Task<int> Update(T entity);

        Task<int> DeleteAsync(T entity);
        Task<int> DeleteByRange(IEnumerable<T> entities);

    }
}
