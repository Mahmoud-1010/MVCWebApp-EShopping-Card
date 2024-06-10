using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

namespace PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        public IUnitOfWork _unitOfWork { get; }
        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<OrderHeader> orderHeaders = await _unitOfWork.OrderHeaderRepository.GetAllAsync(IncludeWord: "ApplicationUser");

            return View(orderHeaders);
        }
        public async Task<IActionResult> GetData()
        {
            IEnumerable<OrderHeader> orderHeaders = await _unitOfWork.OrderHeaderRepository.GetAllAsync(IncludeWord:"ApplicationUser");

            return Json(new { data = orderHeaders });
        }

        public async Task<IActionResult> Details(int orderId)
        {
            OrderViewModel orderViewModel = new OrderViewModel()
            {
                OrderHeader = await _unitOfWork.OrderHeaderRepository.GetByIdAsync(U => U.Id == orderId, IncludeWord: "ApplicationUser"),
                OrderDetails = await _unitOfWork.OrderDetailsRepository.GetAllAsync(X => X.OrderHeaderId == orderId, IncludeWord: "Product")
            };
            return View(orderViewModel);
        }
    }
}
