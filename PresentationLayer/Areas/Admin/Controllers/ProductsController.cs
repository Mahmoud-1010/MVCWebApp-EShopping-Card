using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.Models;

namespace PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        public IUnitOfWork _UnitOfWork { get; }
        public IMapper Mapper { get; }
        public IWebHostEnvironment _webHostEnvironment { get; }

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper,IWebHostEnvironment webHostEnvironment)
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
             
            Console.WriteLine();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel productVM,IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;
                if(file != null) 
                {
                    string fileName = Guid.NewGuid().ToString();
                    var Upload = Path.Combine(RootPath, @"Images/Products/");
                    var ext = Path.GetExtension(file.FileName);
                    using (var fileStream = new FileStream(Path.Combine(Upload, fileName, ext),FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.ImageUrl = @"Images/Products/" + fileName + ext;
                }
                
                Product MappedProduct = Mapper.Map<ProductViewModel,Product>(productVM);
                
                await _UnitOfWork.ProductRepository.InsertAsync(MappedProduct);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.categories = await _UnitOfWork.CategoryRepository.GetAllAsync();
            return View(productVM);
        }
        //public IActionResult testCreate()
        //{
        //    List<Category> Categories =  GetAllAsync();
        //    ViewBag.categories = Categories;
        //    return View();
        //}


    }
}
