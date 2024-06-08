using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using System.Security.Claims;

namespace PresentationLayer.Areas.Customer.Controllers
{
    [Authorize]
    [Area("Customer")]
    public class CartController : Controller
    {
        public IUnitOfWork _unitOfWork { get; }
        public ShoppingCartViewModel ShoppingCartViewModel { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartViewModel = new ShoppingCartViewModel()
            {
                CartsList = await _unitOfWork.ShoppingCartRepository.
                    GetAllAsync(U => U.ApplicationUserId == claim.Value, IncludeWord: "Product")
            };
            foreach (var item in ShoppingCartViewModel.CartsList)
            {
                ShoppingCartViewModel.CartTotalPrice += (item.Count * item.Product.Price);
            }
            if(ShoppingCartViewModel.CartsList.Count()==0) 
                return RedirectToAction(nameof(Index),"Home");

            return View(ShoppingCartViewModel);
        }
        public async Task<IActionResult> Plus(int id)
        {
            ShoppingCart shoppingCart = await _unitOfWork.ShoppingCartRepository.GetByIdAsync(C => C.Id == id);
            _unitOfWork.ShoppingCartRepository.IncreaseCount(shoppingCart, 1);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Minus(int id)
        {
            ShoppingCart shoppingCart = await _unitOfWork.ShoppingCartRepository.GetByIdAsync(C => C.Id == id);
            if (shoppingCart.Count <= 1)
            {
                await _unitOfWork.ShoppingCartRepository.DeleteAsync(shoppingCart);
                
            }
            else
            {
                _unitOfWork.ShoppingCartRepository.DecreaseCount(shoppingCart, 1);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoeItem(int id)
        {
            ShoppingCart shoppingCart = await _unitOfWork.ShoppingCartRepository.
                GetByIdAsync(C => C.Id == id);

            await _unitOfWork.ShoppingCartRepository.DeleteAsync(shoppingCart);
            return RedirectToAction(nameof(Index));

        }
    }
}
