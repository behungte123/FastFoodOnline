using FastFoodOnline.Data;
using FastFoodOnline.Filters;
using FastFoodOnline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FastFoodOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthorize]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            var foods = await _context.Foods
                .Include(f => f.Category)
                .OrderByDescending(f => f.Id)
                .ToListAsync();

            ViewBag.Categories = await _context.FoodCategories.ToListAsync();
            return View(foods);
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var food = await _context.Foods
                .Include(f => f.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            ViewBag.Categories = _context.FoodCategories.ToList();
            return View();
        }

        // POST: Admin/Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Food food)
        {
            if (ModelState.IsValid)
            {
                _context.Add(food);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Thêm sản phẩm thành công!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = _context.FoodCategories.ToList();
            return View(food);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var food = await _context.Foods.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }

            ViewBag.Categories = _context.FoodCategories.ToList();
            return View(food);
        }

        // POST: Admin/Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Food food)
        {
            if (id != food.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(food);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật sản phẩm thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodExists(food.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = _context.FoodCategories.ToList();
            return View(food);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var food = await _context.Foods
                .Include(f => f.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var food = await _context.Foods.FindAsync(id);
            if (food != null)
            {
                _context.Foods.Remove(food);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Xóa sản phẩm thành công!";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Products/Combos
        public async Task<IActionResult> Combos()
        {
            var combos = await _context.Combos
                .Include(c => c.Items)
                .ThenInclude(ci => ci.Food)
                .OrderByDescending(c => c.Id)
                .ToListAsync();

            return View(combos);
        }

        // GET: Admin/Products/ComboDetails/5
        public async Task<IActionResult> ComboDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var combo = await _context.Combos
                .Include(c => c.Items)
                .ThenInclude(ci => ci.Food)
                .ThenInclude(f => f.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (combo == null)
            {
                return NotFound();
            }

            return View(combo);
        }

        // GET: Admin/Products/CreateCombo
        public IActionResult CreateCombo()
        {
            ViewBag.Foods = _context.Foods
                .Include(f => f.Category)
                .Where(f => f.IsActive)
                .OrderBy(f => f.Name)
                .ToList();
            return View();
        }

        // POST: Admin/Products/CreateCombo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCombo(Combo combo, int[] foodIds, int[] quantities)
        {
            if (ModelState.IsValid)
            {
                _context.Add(combo);
                await _context.SaveChangesAsync();

                // Add combo items
                if (foodIds != null && quantities != null && foodIds.Length == quantities.Length)
                {
                    for (int i = 0; i < foodIds.Length; i++)
                    {
                        if (quantities[i] > 0)
                        {
                            var comboItem = new ComboItem
                            {
                                ComboId = combo.Id,
                                FoodId = foodIds[i],
                                Quantity = quantities[i]
                            };
                            _context.ComboItems.Add(comboItem);
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                TempData["Success"] = "Thêm combo thành công!";
                return RedirectToAction(nameof(Combos));
            }

            ViewBag.Foods = _context.Foods
                .Include(f => f.Category)
                .Where(f => f.IsActive)
                .OrderBy(f => f.Name)
                .ToList();
            return View(combo);
        }

        // GET: Admin/Products/EditCombo/5
        public async Task<IActionResult> EditCombo(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var combo = await _context.Combos
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (combo == null)
            {
                return NotFound();
            }

            ViewBag.Foods = _context.Foods
                .Include(f => f.Category)
                .Where(f => f.IsActive)
                .OrderBy(f => f.Name)
                .ToList();
            return View(combo);
        }

        // POST: Admin/Products/EditCombo/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCombo(int id, Combo combo, int[] foodIds, int[] quantities)
        {
            if (id != combo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(combo);

                    // Remove old combo items
                    var oldItems = _context.ComboItems.Where(ci => ci.ComboId == combo.Id);
                    _context.ComboItems.RemoveRange(oldItems);

                    // Add new combo items
                    if (foodIds != null && quantities != null && foodIds.Length == quantities.Length)
                    {
                        for (int i = 0; i < foodIds.Length; i++)
                        {
                            if (quantities[i] > 0)
                            {
                                var comboItem = new ComboItem
                                {
                                    ComboId = combo.Id,
                                    FoodId = foodIds[i],
                                    Quantity = quantities[i]
                                };
                                _context.ComboItems.Add(comboItem);
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật combo thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComboExists(combo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Combos));
            }

            ViewBag.Foods = _context.Foods
                .Include(f => f.Category)
                .Where(f => f.IsActive)
                .OrderBy(f => f.Name)
                .ToList();
            return View(combo);
        }

        // GET: Admin/Products/DeleteCombo/5
        public async Task<IActionResult> DeleteCombo(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var combo = await _context.Combos
                .Include(c => c.Items)
                .ThenInclude(ci => ci.Food)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (combo == null)
            {
                return NotFound();
            }

            return View(combo);
        }

        // POST: Admin/Products/DeleteCombo/5
        [HttpPost, ActionName("DeleteCombo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComboConfirmed(int id)
        {
            var combo = await _context.Combos.FindAsync(id);
            if (combo != null)
            {
                _context.Combos.Remove(combo);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Xóa combo thành công!";
            }

            return RedirectToAction(nameof(Combos));
        }

        private bool FoodExists(int id)
        {
            return _context.Foods.Any(e => e.Id == id);
        }

        private bool ComboExists(int id)
        {
            return _context.Combos.Any(e => e.Id == id);
        }
    }
}
