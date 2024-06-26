﻿using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        //Task<int> UpdateAsync(Product product);
        public Task<IEnumerable<Product>> Search(string Name);

    }
}
