using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Helper;
using PresentationLayer.Models;

namespace PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly IMapper _mapper;

        public IUnitOfWork _UnitOfWork { get; }
        public DashboardController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _UnitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var allOrders = await _UnitOfWork.OrderHeaderRepository.GetAllAsync();
            ViewBag.AllOrders = allOrders.Count();
            var approvedOrders =await _UnitOfWork.OrderHeaderRepository.GetAllAsync(x => x.OrderStatus == SD.Approve);
            ViewBag.ApprovedOrders =   approvedOrders.Count();
            var users = await _UnitOfWork.ApplicationUserRepository.GetAllAsync();
            ViewBag.UsersRegistration= users.Count();
            var products = await _UnitOfWork.ProductRepository.GetAllAsync();
            ViewBag.Products=products.Count();

            return View();
        }
    }
}
