using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using System.Security.Claims;

namespace PresentationLayer.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        public IUnitOfWork _unitOfWork { get; }
        public IMapper Mapper { get; }

        public HomeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            Mapper = mapper;
        }


        public async Task<IActionResult> Index()
        {
            var products = await _unitOfWork.ProductRepository.GetAllAsync();
            var mappedProducts = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(products);
            return View(mappedProducts);
        }
        public async Task<IActionResult> Details(int id)
        {
            var product =await _unitOfWork.ProductRepository.GetByIdAsync(P=>P.Id==id,IncludeWord:"Category");
            ShoppingItemViewModel shoppingItem = Mapper.Map<Product, ShoppingItemViewModel>(product);
            shoppingItem.ProductId = id;
            return View(shoppingItem);
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(ShoppingItemViewModel shoppingItem)
        {
            //var product = await _unitOfWork.ProductRepository.GetByIdAsync(P => P.Id == id);
            //ShoppingItemViewModel shoppingItem = Mapper.Map<Product, ShoppingItemViewModel>(product);
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingItem.ApplicationUserId = claim.Value;
            ShoppingCart shoppingCart = Mapper.Map<ShoppingItemViewModel, ShoppingCart>(shoppingItem);
            await _unitOfWork.ShoppingCartRepository.InsertAsync(shoppingCart);
            
            return RedirectToAction(nameof(Index));
        }
    }
}
