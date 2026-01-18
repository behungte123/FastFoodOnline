using FastFood.Services;
using FastFoodOnline.Data;
using FastFoodOnline.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FastFoodOnline.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;

        public AccountController(AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // ==========================================
        // KHU VỰC LOGIN GOOGLE
        // ==========================================

        [HttpGet]
        public IActionResult LoginByGoogle()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                TempData["Error"] = "Đăng nhập Google thất bại.";
                return RedirectToAction("Login");
            }

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var fullName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "Lỗi: Không lấy được email từ Google.";
                return RedirectToAction("Login");
            }

            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);

            if (account == null)
            {
                // User mới từ Google -> Tự động kích hoạt luôn (IsActive = true)
                account = new Account
                {
                    Email = email,
                    FullName = fullName ?? "Google User",
                    RoleId = 2,
                    Password = Guid.NewGuid().ToString(),
                    PhoneNumber = "",
                    Address = "",
                    DateOfBirth = DateTime.Now,
                    IsActive = true, // <--- ĐIỂM QUAN TRỌNG
                    ActivationToken = null
                };
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Nếu tài khoản đã có nhưng chưa kích hoạt (do đăng ký tay trước đó)
                // Mà giờ họ đăng nhập bằng Google -> Kích hoạt luôn cho họ
                if (!account.IsActive)
                {
                    account.IsActive = true;
                    account.ActivationToken = null;
                    await _context.SaveChangesAsync();
                }
            }

            // Gán Session
            HttpContext.Session.SetInt32("UserId", account.Id);
            HttpContext.Session.SetString("UserName", account.FullName);
            HttpContext.Session.SetString("UserEmail", account.Email);
            HttpContext.Session.SetInt32("UserRole", account.RoleId);

            // Gửi mail cảnh báo bảo mật
            try
            {
                string subject = "[Cảnh báo] Đăng nhập mới qua Google";
                string content = $"Xin chào {account.FullName}, tài khoản của bạn vừa đăng nhập qua Google lúc {DateTime.Now}.";
                _ = _emailService.SendEmailAsync(email, subject, content);
            }
            catch { }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (account.RoleId == 1)
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

            return RedirectToAction("Index", "Home");
        }

        // ==========================================
        // KHU VỰC ĐĂNG KÝ & ĐĂNG NHẬP THƯỜNG
        // ==========================================

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (await _context.Accounts.AnyAsync(a => a.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email đã được sử dụng.");
                return View(model);
            }

            var account = new Account
            {
                FullName = model.FullName,
                Email = model.Email,
                Password = model.Password,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                DateOfBirth = model.DateOfBirth,
                RoleId = 2,

                // --- ĐIỂM QUAN TRỌNG ---
                IsActive = false, // Chưa kích hoạt
                ActivationToken = Guid.NewGuid().ToString() // Tạo mã kích hoạt
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            // --- GỬI MAIL KÍCH HOẠT ---
            try
            {
                // Tạo link kích hoạt trỏ về hàm Verify bên dưới
                var verifyUrl = Url.Action("Verify", "Account", new { token = account.ActivationToken }, Request.Scheme);

                string subject = "Xác thực tài khoản FastFood Online";
                string content = $@"
                    <h3>Xin chào {model.FullName},</h3>
                    <p>Cảm ơn bạn đã đăng ký. Vui lòng bấm vào link dưới đây để kích hoạt tài khoản:</p>
                    <p><a href='{verifyUrl}' style='padding: 10px 20px; background-color: #E4002B; color: white; text-decoration: none; border-radius: 5px;'>KÍCH HOẠT TÀI KHOẢN</a></p>
                    <p>Nếu bạn không đăng ký tài khoản này, vui lòng bỏ qua email này.</p>";

                _ = _emailService.SendEmailAsync(model.Email, subject, content);
            }
            catch { }

            TempData["Success"] = "Đăng ký thành công! Vui lòng kiểm tra Email để kích hoạt tài khoản.";
            return RedirectToAction("Login");
        }

        // Action Xử lý khi người dùng bấm Link trong Email (MỚI THÊM)
        [HttpGet]
        public async Task<IActionResult> Verify(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "Mã xác thực không hợp lệ.";
                return RedirectToAction("Login");
            }

            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.ActivationToken == token);

            if (account == null)
            {
                TempData["Error"] = "Mã xác thực không đúng hoặc đã hết hạn.";
                return RedirectToAction("Login");
            }

            // Kích hoạt tài khoản
            account.IsActive = true;
            account.ActivationToken = null; // Xóa token đi để không dùng lại được
            await _context.SaveChangesAsync();

            TempData["Success"] = "Kích hoạt tài khoản thành công! Bạn có thể đăng nhập ngay.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var account = await _context.Accounts.FirstOrDefaultAsync(a =>
                a.Email == model.Email && a.Password == model.Password);

            if (account == null)
            {
                ModelState.AddModelError("", "Email hoặc mật khẩu không chính xác.");
                return View(model);
            }

            // --- KIỂM TRA KÍCH HOẠT ---
            if (account.IsActive == false)
            {
                ModelState.AddModelError("", "Tài khoản chưa được kích hoạt. Vui lòng kiểm tra email của bạn.");
                return View(model);
            }

            // Gán Session
            HttpContext.Session.SetInt32("UserId", account.Id);
            HttpContext.Session.SetString("UserName", account.FullName);
            HttpContext.Session.SetString("UserEmail", account.Email);
            HttpContext.Session.SetInt32("UserRole", account.RoleId);

            TempData["Success"] = $"Chào mừng {account.FullName}!";

            if (account.RoleId == 1)
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

            return RedirectToAction("Index", "Home");
        }

        // ==========================================
        // CÁC CHỨC NĂNG KHÁC (GIỮ NGUYÊN)
        // ==========================================

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "Bạn đã đăng xuất thành công.";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View();

        public IActionResult Profile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login");

            var account = _context.Accounts.Include(a => a.Role).FirstOrDefault(a => a.Id == userId);
            return account == null ? RedirectToAction("Login") : View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(Account model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login");

            var account = await _context.Accounts.FindAsync(userId);
            if (account == null) return RedirectToAction("Login");

            account.FullName = model.FullName;
            account.PhoneNumber = model.PhoneNumber;
            account.Address = model.Address;
            account.DateOfBirth = model.DateOfBirth;

            if (!string.IsNullOrEmpty(model.Password)) account.Password = model.Password;

            await _context.SaveChangesAsync();
            HttpContext.Session.SetString("UserName", account.FullName);

            TempData["Success"] = "Cập nhật thông tin thành công!";
            return RedirectToAction("Profile");
        }

        public IActionResult MyOrders()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login");

            var orders = _context.Orders
                .Where(o => o.AccountId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

        public IActionResult OrderDetails(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login");

            var order = _context.Orders
                .Include(o => o.Items).ThenInclude(od => od.Food)
                .Include(o => o.Items).ThenInclude(od => od.Combo)
                .FirstOrDefault(o => o.Id == id && o.AccountId == userId);

            return order == null ? NotFound() : View(order);
        }
    }
}