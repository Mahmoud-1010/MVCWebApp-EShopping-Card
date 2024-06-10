using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Helper;
using PresentationLayer.Models;
using Stripe.Checkout;
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
            if (ShoppingCartViewModel.CartsList.Count() == 0)
                return RedirectToAction(nameof(Index), "Home");

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
        [HttpGet]
        public async Task<IActionResult> SummaryUi()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartViewModel = new ShoppingCartViewModel()
            {
                CartsList = await _unitOfWork.ShoppingCartRepository.
                    GetAllAsync(U => U.ApplicationUserId == claim.Value, IncludeWord: "Product"),
                OrderHeader = new()

            };
            ShoppingCartViewModel.OrderHeader.ApplicationUser = await _unitOfWork.ApplicationUserRepository.
                GetByIdAsync(U => U.Id == claim.Value);

            ShoppingCartViewModel.OrderHeader.Name =ShoppingCartViewModel.OrderHeader.ApplicationUser.Name;
            ShoppingCartViewModel.OrderHeader.Address =ShoppingCartViewModel.OrderHeader.ApplicationUser.Address;
            ShoppingCartViewModel.OrderHeader.City =ShoppingCartViewModel.OrderHeader.ApplicationUser.City;
            ShoppingCartViewModel.OrderHeader.Phone =ShoppingCartViewModel.OrderHeader.ApplicationUser.PhoneNumber;


            foreach (var item in ShoppingCartViewModel.CartsList)
            {
                ShoppingCartViewModel.CartTotalPrice += (item.Count * item.Product.Price);
            }
            return View(ShoppingCartViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SummaryUi(ShoppingCartViewModel shoppingCartViewModel)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppingCartViewModel.CartsList = await _unitOfWork.ShoppingCartRepository.
                    GetAllAsync(U => U.ApplicationUserId == claim.Value, IncludeWord: "Product");

            //shoppingCartViewModel.CartsList = await _unitOfWork.ShoppingCartRepository.
            //        GetAllAsync(U => U.ApplicationUserId == claim.Value);
            //ShoppingCartViewModel = new ShoppingCartViewModel()
            //{
            //    CartsList = await _unitOfWork.ShoppingCartRepository.
            //        GetAllAsync(U => U.ApplicationUserId == claim.Value, IncludeWord: "Product"),
            //    OrderHeader = new()

            //};
            //shoppingCartViewModel = ShoppingCartViewModel;


            shoppingCartViewModel.OrderHeader.OrderStatus = SD.Pending;
            shoppingCartViewModel.OrderHeader.PaymentStatus = SD.Pending;
            shoppingCartViewModel.OrderHeader.OrderDate = DateTime.Now;
            shoppingCartViewModel.OrderHeader.ApplicationUserId = claim.Value;


            //foreach (var item in ShoppingCartViewModel.CartsList)
            //{
            //    ShoppingCartViewModel.CartTotalPrice += (item.Count * item.Product.Price);
            //}


            await _unitOfWork.OrderHeaderRepository.InsertAsync(shoppingCartViewModel.OrderHeader);

            foreach (var item in shoppingCartViewModel.CartsList)
            {
                OrderDetails orderDetails = new OrderDetails()
                {
                    ProductId = item.ProductId,
                    OrderHeaderId = shoppingCartViewModel.OrderHeader.Id,
                    Price = item.Product.Price,
                    Count = item.Count
                };
                await _unitOfWork.OrderDetailsRepository.InsertAsync(orderDetails);
            }

            var domain = "https://localhost:44356/";


            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"Customer/Cart/OrderConfirmation?id={shoppingCartViewModel.OrderHeader.Id}",
                CancelUrl = domain + $"Customer/Cart/Index",
            };
            foreach (var item in shoppingCartViewModel.CartsList)
            {
                var sessionlineoption = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)item.Product.Price * 100,
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        },

                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionlineoption);
            }


            var service = new SessionService();
            Session session = service.Create(options);
            shoppingCartViewModel.OrderHeader.SessionId = session.Id;
            shoppingCartViewModel.OrderHeader.PaymentIntentId = session.PaymentIntentId;
            await _unitOfWork.OrderHeaderRepository.Update(shoppingCartViewModel.OrderHeader);
            //_unitOfWork.Complete();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
            //return Content($"{shoppingCartViewModel.OrderHeader.ApplicationUserId}");
        }

        public async Task<IActionResult> OrderConfirmation(int id)
        {
            OrderHeader orderHeader = await _unitOfWork.OrderHeaderRepository.GetByIdAsync(U => U.Id == id);
            var services = new SessionService();
            Session session = services.Get(orderHeader.SessionId);
            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.OrderHeaderRepository.UpdateOrderStatus(id, SD.Approve, SD.Approve);
                //orderHeader.PaymentIntentId = session.PaymentIntentId;
                //_unitOfWork.Complete();
            }

            var shoppingCarts = await _unitOfWork.ShoppingCartRepository.
                GetAllAsync(U => U.ApplicationUserId == orderHeader.ApplicationUserId);
            await _unitOfWork.ShoppingCartRepository.DeleteByRange(shoppingCarts);
            return View(id);
        }
    }
}
