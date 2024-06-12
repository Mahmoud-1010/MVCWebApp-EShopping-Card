using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Helper;
using PresentationLayer.Models;
using Stripe;

namespace PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class OrdersController : Controller
    {
        public IUnitOfWork _unitOfWork { get; }
        [BindProperty]
        public OrderViewModel orderViewModel { get; set; }
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
            IEnumerable<OrderHeader> orderHeaders = await _unitOfWork.OrderHeaderRepository.GetAllAsync(IncludeWord: "ApplicationUser");

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderDetails()
        {
            var orderfromDb = await _unitOfWork.OrderHeaderRepository.
                GetByIdAsync(U => U.Id == orderViewModel.OrderHeader.Id);
            orderfromDb.Name = orderViewModel.OrderHeader.Name;
            orderfromDb.Phone = orderViewModel.OrderHeader.Phone;
            orderfromDb.Address = orderViewModel.OrderHeader.Address;
            orderfromDb.City = orderViewModel.OrderHeader.City;
            if (orderViewModel.OrderHeader.Carrier != null)
                orderfromDb.Carrier = orderViewModel.OrderHeader.Carrier;
            if (orderViewModel.OrderHeader.TrackingNumber != null)
                orderfromDb.TrackingNumber = orderViewModel.OrderHeader.TrackingNumber;
            await _unitOfWork.OrderHeaderRepository.Update(orderfromDb);
            return RedirectToAction(nameof(Details), "Orders", new { orderId = orderfromDb.Id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartProccess()
        {
            _unitOfWork.OrderHeaderRepository.UpdateOrderStatus(orderViewModel.OrderHeader.Id, SD.Proccessing, null);
            return RedirectToAction(nameof(Details), "Orders", new { orderId = orderViewModel.OrderHeader.Id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartShipping()
        {
            var orderHeaderFromDb = await _unitOfWork.OrderHeaderRepository.GetByIdAsync(U => U.Id == orderViewModel.OrderHeader.Id);
            orderHeaderFromDb.TrackingNumber = orderViewModel.OrderHeader.TrackingNumber;
            orderHeaderFromDb.Carrier = orderViewModel.OrderHeader.Carrier;
            orderHeaderFromDb.ShippingDate = DateTime.Now;
            orderHeaderFromDb.OrderStatus = SD.Shipping;
            await _unitOfWork.OrderHeaderRepository.Update(orderHeaderFromDb);




            return RedirectToAction(nameof(Details), "Orders", new { orderId = orderViewModel.OrderHeader.Id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder()
        {
            var orderHeaderFromDb = await _unitOfWork.OrderHeaderRepository.GetByIdAsync(U => U.Id == orderViewModel.OrderHeader.Id);
            if(orderHeaderFromDb.PaymentStatus == SD.Approve)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeaderFromDb.PaymentIntentId
                };
                var service = new RefundService();
                Refund refund = service.Create(options);
                _unitOfWork.OrderHeaderRepository.UpdateOrderStatus(orderHeaderFromDb.Id, SD.Cancelleed, SD.Refund);
            }
            else
            {
                _unitOfWork.OrderHeaderRepository.UpdateOrderStatus(orderHeaderFromDb.Id, SD.Cancelleed, SD.Cancelleed);
            }
            

            return RedirectToAction(nameof(Details), "Orders", new { orderId = orderViewModel.OrderHeader.Id });
        }

    }
}
