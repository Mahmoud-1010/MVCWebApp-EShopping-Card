using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        public IUnitOfWork _UnitOfWork { get; }

        public ProductsController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
