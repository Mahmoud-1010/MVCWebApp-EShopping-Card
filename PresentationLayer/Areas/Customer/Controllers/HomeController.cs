using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.Customer.Controllers
{
    public class HomeController : Controller
    {
        public IUnitOfWork _unitOfWork { get; }

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<IActionResult> Index()
        {
            var products = await _unitOfWork.ProductRepository.GetAllAsync();
            return View(products);
        }
    }
}
