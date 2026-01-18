using FastFoodOnline.Data;
using FastFoodOnline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FastFoodOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var popularFoods = _context.Foods
                .Include(f => f.Category)
                .Where(f => f.IsActive)
                .OrderBy(f => f.Name)
                .Take(8)
                .ToList();

            var combos = _context.Combos
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .Take(3)
                .ToList();

            var vm = new HomeViewModel
            {
                PopularFoods = popularFoods,
                Combos = combos
            };

            return View(vm);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
