using FastFoodOnline.Data;
using FastFoodOnline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FastFoodOnline.Controllers
{
    public class FoodsController : Controller
    {
        private readonly AppDbContext _context;

        public FoodsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Foods
        // Thêm tham số pageNumber vào cuối
        public async Task<IActionResult> Index(string? search, int? categoryId, decimal? minPrice, decimal? maxPrice, string? tag, int? pageNumber)
        {
            // 1. Giữ nguyên Logic lọc dữ liệu
            var query = _context.Foods
                .Include(f => f.Category)
                .Where(f => f.IsActive)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(f => f.Name.Contains(search));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(f => f.CategoryId == categoryId.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(f => f.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {   
                query = query.Where(f => f.Price <= maxPrice.Value);
            }

            if (!string.IsNullOrWhiteSpace(tag))
            {
                query = query.Where(f => f.Tag != null && f.Tag.Contains(tag));
            }

            // Sắp xếp trước khi phân trang (Bắt buộc)
            query = query.OrderBy(f => f.Name);

            // 2. Cấu hình phân trang
            int pageSize = 12; // 10 sản phẩm 1 trang

            // 3. Thực thi query và cắt trang
            // Lưu ý: Không dùng .ToList() nữa mà dùng CreateAsync của class vừa tạo
            var foods = await PaginatedList<Food>.CreateAsync(query.AsNoTracking(), pageNumber ?? 1, pageSize);

            // 4. Lưu lại các giá trị lọc vào ViewBag để giữ lại khi chuyển trang
            ViewBag.Search = search;
            ViewBag.CategoryId = categoryId;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.Tag = tag;
            ViewBag.Categories = _context.FoodCategories.OrderBy(c => c.Name).ToList();

            return View(foods);
        }

        // GET: /Foods/Details/5
        public IActionResult Details(int id)
        {
            var food = _context.Foods
                .Include(f => f.Category)
                .FirstOrDefault(f => f.Id == id);

            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }
    }
}