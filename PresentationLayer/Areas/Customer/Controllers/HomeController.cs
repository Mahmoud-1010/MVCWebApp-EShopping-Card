using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Helper;
using PresentationLayer.Models;
using System.Security.Claims;
using X.PagedList;

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


        public async Task<IActionResult> Index(int? page)
        {
            int PageNumber = page ?? 1;
            int PageSize = 8;
            var products = await _unitOfWork.ProductRepository.GetAllAsync();
            var mappedProducts = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(products);
            return View(mappedProducts.ToPagedList(PageNumber,PageSize));
        }
        public async Task<IActionResult> Details(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(P => P.Id == id, IncludeWord: "Category");
            ShoppingItemViewModel shoppingItem = Mapper.Map<Product, ShoppingItemViewModel>(product);
            shoppingItem.ProductId = id;
            return View(shoppingItem);
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(ShoppingItemViewModel shoppingItem)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingItem.ApplicationUserId = claim.Value;
            ShoppingCart shoppingCart = Mapper.Map<ShoppingItemViewModel, ShoppingCart>(shoppingItem);

            ShoppingCart cartobj = await _unitOfWork.ShoppingCartRepository.GetByIdAsync(
                U => U.ApplicationUserId == claim.Value &&
                     U.ProductId == shoppingCart.ProductId);

            if (cartobj == null)
            {
                await _unitOfWork.ShoppingCartRepository.InsertAsync(shoppingCart);
                var shoppingCarts = await _unitOfWork.ShoppingCartRepository.
                        GetAllAsync(x => x.ApplicationUserId == claim.Value);
                var mapedShoppingCart = Mapper.Map<IEnumerable<ShoppingCart>, List<ShoppingItemViewModel>>(shoppingCarts);
                HttpContext.Session.SetInt32(SD.SessionKey, mapedShoppingCart.Count());
                 //_unitOfWork.Complete();
            }
            else
            {
               _unitOfWork.ShoppingCartRepository.IncreaseCount(cartobj, shoppingCart.Count);

                //await _unitOfWork.ShoppingCartRepository.Update(shoppingCart);
            }
            //_unitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }
    }
}
