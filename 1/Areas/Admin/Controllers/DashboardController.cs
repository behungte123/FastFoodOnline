using FastFoodOnline.Data;
using FastFoodOnline.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FastFoodOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthorize]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Statistics for dashboard
            var totalAccounts = _context.Accounts.Count();
            var totalCustomers = _context.Accounts.Count(a => a.RoleId == 2);
            var totalOrders = _context.Orders.Count();
            var totalRevenue = _context.Orders.Sum(o => o.TotalAmount);
            var totalFoods = _context.Foods.Count();
            var totalCombos = _context.Combos.Count();
            var pendingOrders = _context.Orders.Count(o => o.Status == "Pending");
            var processingOrders = _context.Orders.Count(o => o.Status == "Processing");

            ViewBag.TotalAccounts = totalAccounts;
            ViewBag.TotalCustomers = totalCustomers;
            ViewBag.TotalOrders = totalOrders;
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.TotalFoods = totalFoods;
            ViewBag.TotalCombos = totalCombos;
            ViewBag.PendingOrders = pendingOrders;
            ViewBag.ProcessingOrders = processingOrders;

            // Recent orders
            var recentOrders = _context.Orders
                .Include(o => o.Account)
                .OrderByDescending(o => o.OrderDate)
                .Take(10)
                .ToList();

            return View(recentOrders);
        }
    }
}
