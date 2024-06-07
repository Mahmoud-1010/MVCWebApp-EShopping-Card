using DataAccessLayer.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Helper;
using System.Security.Claims;

namespace PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        public ApplicationDBContext _context { get; }

        public UsersController(ApplicationDBContext applicationDBContext)
        {
            _context = applicationDBContext;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = claim.Value;
            return View(_context.ApplicationUsers.Where(X => X.Id != userId).ToList());
        }
        public IActionResult LockUnLock(string id)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(U => U.Id == id);
            if (user == null)
                return NotFound();
            if (user.LockoutEnd == null || user.LockoutEnd < DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now.AddMonths(1);
            }
            else
            {
                user.LockoutEnd = DateTime.Now;
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
