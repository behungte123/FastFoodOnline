using FastFoodOnline.Data;
using FastFoodOnline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FastFoodOnline.Controllers
{
    public class CombosController : Controller
    {
        private readonly AppDbContext _context;

        public CombosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Combos
        public IActionResult Index()
        {
            var combos = _context.Combos
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToList();

            return View(combos);
        }

        // GET: /Combos/Details/5
        public IActionResult Details(int id)
        {
            var combo = _context.Combos
                .Include(c => c.Items)
                    .ThenInclude(i => i.Food)
                        .ThenInclude(f => f.Category)
                .FirstOrDefault(c => c.Id == id);

            if (combo == null)
            {
                return NotFound();
            }

            return View(combo);
        }
    }
}
