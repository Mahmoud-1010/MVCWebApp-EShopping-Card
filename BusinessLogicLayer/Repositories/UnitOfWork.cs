﻿using BusinessLogicLayer.Interfaces;
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
        public ICategoryRepository CategoryRepository { get; private set; }
        public IProductRepository ProductRepository { get; private set; }

        public UnitOfWork(ICategoryRepository categoryRepository,IProductRepository productRepository)
        {
            CategoryRepository = categoryRepository;
            ProductRepository = productRepository;
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