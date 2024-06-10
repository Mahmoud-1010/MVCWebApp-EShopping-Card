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
        public ICategoryRepository CategoryRepository { get; private set; }
        public IProductRepository ProductRepository { get; private set; }

        public IShoppingCartRepository ShoppingCartRepository {  get; private set; }

        public IOrderHeaderRepository OrderHeaderRepository {  get; private set; }
        public IOrderDetailsRepository OrderDetailsRepository { get; private set; }

        public IApplicationUserRepository ApplicationUserRepository { get; private set; }

        public UnitOfWork(ICategoryRepository categoryRepository,
            IProductRepository productRepository, 
            IShoppingCartRepository shoppingCartRepository,
            IOrderHeaderRepository orderHeaderRepository,
            IOrderDetailsRepository orderDetailsRepository,
            IApplicationUserRepository applicationUserRepository)
        {
            CategoryRepository = categoryRepository;
            ProductRepository = productRepository;
            ShoppingCartRepository = shoppingCartRepository;
            OrderHeaderRepository = orderHeaderRepository;
            OrderDetailsRepository = orderDetailsRepository;
            ApplicationUserRepository = applicationUserRepository;
        }


        public int Complete()
        {
           return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        //public void Dispose()
        //{
        //    
        //}
    }
}
