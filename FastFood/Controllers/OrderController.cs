using FastFood.Services;
using FastFoodOnline.Data;
using FastFoodOnline.Helpers;
using FastFoodOnline.Models;
// <-- Đảm bảo namespace này đúng với chỗ bạn để file EmailService
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks; // Cần thiết cho async/await

namespace FastFoodOnline.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService; // 1. Khai báo service gửi mail

        // 2. Tiêm (Inject) IEmailService vào constructor
        public OrderController(AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        public IActionResult AddFoodToCart(int id, int quantity = 1)
        {
            var food = _context.Foods.Find(id);
            if (food == null || !food.IsActive)
            {
                TempData["Error"] = "Món ăn không tồn tại hoặc đã ngưng kinh doanh.";
                return RedirectToAction("Index", "Foods");
            }

            var cartItem = new CartItem
            {
                Id = food.Id,
                Name = food.Name,
                Price = food.Price,
                Quantity = quantity,
                ImageUrl = food.ImageUrl,
                Type = "Food"
            };

            CartHelper.AddToCart(HttpContext.Session, cartItem);
            TempData["Success"] = $"Đã thêm '{food.Name}' vào giỏ hàng!";

            // Quay lại trang trước đó (trang danh sách món ăn)
            return Redirect(Request.Headers["Referer"].ToString());
        }

        // Add combo to cart
        [HttpPost]
        public IActionResult AddComboToCart(int id, int quantity = 1)
        {
            var combo = _context.Combos.Find(id);
            if (combo == null || !combo.IsActive)
            {
                TempData["Error"] = "Combo không tồn tại hoặc đã ngưng kinh doanh.";
                return RedirectToAction("Index", "Combos");
            }

            var cartItem = new CartItem
            {
                Id = combo.Id,
                Name = combo.Name,
                Price = combo.Price,
                Quantity = quantity,
                ImageUrl = combo.ImageUrl,
                Type = "Combo"
            };

            CartHelper.AddToCart(HttpContext.Session, cartItem);
            TempData["Success"] = $"Đã thêm combo '{combo.Name}' vào giỏ hàng!";

            // Quay lại trang trước đó (trang danh sách món ăn)
            return Redirect(Request.Headers["Referer"].ToString());
        }

        // View cart
        public IActionResult Cart()
        {
            var cart = CartHelper.GetCart(HttpContext.Session);
            ViewBag.CartTotal = CartHelper.GetCartTotal(HttpContext.Session);
            return View(cart);
        }

        // Update cart item quantity
        [HttpPost]
        public IActionResult UpdateQuantity(int id, string type, int quantity)
        {
            CartHelper.UpdateQuantity(HttpContext.Session, id, type, quantity);
            return RedirectToAction("Cart");
        }

        // Remove item from cart
        [HttpPost]
        public IActionResult RemoveFromCart(int id, string type)
        {
            CartHelper.RemoveFromCart(HttpContext.Session, id, type);
            TempData["Success"] = "Đã xóa sản phẩm khỏi giỏ hàng.";
            return RedirectToAction("Cart");
        }

        // Clear entire cart
        [HttpPost]
        public IActionResult ClearCart()
        {
            CartHelper.ClearCart(HttpContext.Session);
            TempData["Success"] = "Đã xóa tất cả sản phẩm trong giỏ hàng.";
            return RedirectToAction("Cart");
        }
        // Checkout - requires login
        public IActionResult Checkout()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["Error"] = "Vui lòng đăng nhập để thanh toán.";
                return RedirectToAction("Login", "Account");
            }

            var cart = CartHelper.GetCart(HttpContext.Session);
            if (!cart.Any())
            {
                TempData["Error"] = "Giỏ hàng trống. Vui lòng thêm sản phẩm trước khi thanh toán.";
                return RedirectToAction("Cart");
            }

            ViewBag.CartTotal = CartHelper.GetCartTotal(HttpContext.Session);
            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            return View(cart);
        }

        // ----------------------------------------------------------------------
        // ĐÂY LÀ HÀM QUAN TRỌNG NHẤT ĐÃ ĐƯỢC SỬA ĐỔI
        // ----------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> ProcessCheckout(string deliveryAddress, string notes)
        {
            // 1. Kiểm tra đăng nhập
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["Error"] = "Vui lòng đăng nhập để thanh toán.";
                return RedirectToAction("Login", "Account");
            }

            // 2. Kiểm tra giỏ hàng
            var cart = CartHelper.GetCart(HttpContext.Session);
            if (!cart.Any())
            {
                TempData["Error"] = "Giỏ hàng trống.";
                return RedirectToAction("Cart");
            }

            // 3. Tạo đơn hàng (Order)
            var order = new Order
            {
                AccountId = userId.Value,
                OrderDate = DateTime.Now,
                TotalAmount = CartHelper.GetCartTotal(HttpContext.Session),
                Status = "Pending",
                DeliveryAddress = deliveryAddress,
                Notes = notes
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(); // Dùng await để lưu DB bất đồng bộ

            // 4. Tạo chi tiết đơn hàng (OrderDetail)
            foreach (var item in cart)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    FoodId = item.Type == "Food" ? item.Id : null,
                    ComboId = item.Type == "Combo" ? item.Id : null,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price
                };
                _context.OrderDetails.Add(orderDetail);
            }
            await _context.SaveChangesAsync();

            // -----------------------------------------------------------
            // 5. GỬI EMAIL XÁC NHẬN (Code mới thêm vào)
            // -----------------------------------------------------------
            try
            {
                // Lấy thông tin user để biết email gửi về đâu
                // Giả sử bảng chứa user của bạn tên là Accounts và có trường Email
                var user = await _context.Accounts.FindAsync(userId.Value);

                if (user != null && !string.IsNullOrEmpty(user.Email))
                {
                    string subject = $"[FastFood] Đặt hàng thành công - Mã đơn #{order.Id}";
                    string body = $@"
                        <div style='font-family:Arial, sans-serif; max-width:600px; margin:0 auto;'>
                            <h2 style='color:#d9534f;'>Cảm ơn bạn đã đặt hàng!</h2>
                            <p>Xin chào <b>{user.FullName ?? "Khách hàng"}</b>,</p>
                            <p>Đơn hàng của bạn đã được tiếp nhận và đang xử lý.</p>
                            <hr style='border:1px dashed #ccc;' />
                            <h3>Thông tin đơn hàng #{order.Id}</h3>
                            <p><b>Ngày đặt:</b> {DateTime.Now:dd/MM/yyyy HH:mm}</p>
                            <p><b>Địa chỉ giao:</b> {deliveryAddress}</p>
                            <p><b>Tổng thanh toán:</b> <span style='color:red; font-size:18px; font-weight:bold;'>{order.TotalAmount:N0} VNĐ</span></p>
                            <p><b>Ghi chú:</b> {notes}</p>
                            <hr style='border:1px dashed #ccc;' />
                            <p>Chúng tôi sẽ liên hệ với bạn sớm nhất có thể.</p>
                            <p>Trân trọng,<br/><b>FastFood Online Team</b></p>
                        </div>";

                    // Gọi hàm gửi mail
                    await _emailService.SendEmailAsync(user.Email, subject, body);
                }
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu cần thiết, nhưng KHÔNG throw exception
                // Để đảm bảo dù lỗi mail thì khách vẫn đặt hàng thành công.
                Console.WriteLine("Lỗi gửi mail: " + ex.Message);
            }
            // -----------------------------------------------------------

            // 6. Xóa giỏ hàng
            CartHelper.ClearCart(HttpContext.Session);

            TempData["Success"] = $"Đặt hàng thành công! Mã đơn hàng: #{order.Id}";
            return RedirectToAction("OrderConfirmation", new { id = order.Id });
        }

        // Order confirmation page (Giữ nguyên)
        public IActionResult OrderConfirmation(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = _context.Orders
                .Include(o => o.Items).ThenInclude(od => od.Food)
                .Include(o => o.Items).ThenInclude(od => od.Combo)
                .FirstOrDefault(o => o.Id == id && o.AccountId == userId.Value);

            if (order == null)
            {
                TempData["Error"] = "Không tìm thấy đơn hàng.";
                return RedirectToAction("Index", "Home");
            }

            return View(order);
        }
    }
}