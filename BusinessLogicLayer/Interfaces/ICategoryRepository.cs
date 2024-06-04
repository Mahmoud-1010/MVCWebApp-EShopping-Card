using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface ICategoryRepository:IGenericRepository<Category>
    {
        //Task<int> UpdateAsync(Category category);
        public Task<IEnumerable<Category>> Search(string Name);
    }
}
