using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.Helper;
using PresentationLayer.Models;

namespace PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class ProductsController : Controller
    {
        public IUnitOfWork _UnitOfWork { get; }
        public IMapper Mapper { get; }
        public IWebHostEnvironment _webHostEnvironment { get; }

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _UnitOfWork = unitOfWork;
            Mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<ActionResult> Index(string SearchValue = "")
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
                IEnumerable<Product> products = await _UnitOfWork.ProductRepository.GetAllAsync();
                List<ProductViewModel> productsVM = Mapper.Map<IEnumerable<Product>, List<ProductViewModel>>(products);
                return View(productsVM);
            }
            else
            {
                var products = await _UnitOfWork.ProductRepository.Search(SearchValue);
                var MappedProducts = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(products);
                return View(MappedProducts);
            }
        }



        public async Task<IActionResult> Create()
        {
            ViewBag.categories = await _UnitOfWork.CategoryRepository.GetAllAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel productVM, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                //productVM.CategoryId = 3;
                productVM.ImageUrl= DocumentSettings.UploadFile(productVM.Image, "Images");
                Product MappedProduct = Mapper.Map<ProductViewModel, Product>(productVM);

                await _UnitOfWork.ProductRepository.InsertAsync(MappedProduct);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.categories = await _UnitOfWork.CategoryRepository.GetAllAsync();
            return View(productVM);
        }

        //TO DO
        //Edit
        //Update
        //Delete
    }
}
