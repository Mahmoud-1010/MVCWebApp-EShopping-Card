using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            return View();
        }
        public async Task<IActionResult> GetData()
        {
            IEnumerable<OrderHeader> orderHeaders = await _unitOfWork.OrderHeaderRepository.GetAllAsync(IncludeWord:"ApplicationUser");

            return Json(new { data = orderHeaders });
        }
    }
}
