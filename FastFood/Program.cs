using FastFood.Services;
using FastFoodOnline.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DbContext - EF Core + SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add HttpContextAccessor for session access in views
builder.Services.AddHttpContextAccessor();

// ==========================================
// 1. ĐĂNG KÝ EMAIL SERVICE (MỚI THÊM)
// ==========================================
// Yêu cầu đề bài: Áp dụng Transient service
builder.Services.AddTransient<IEmailService, EmailService>();


// ==========================================
// 2. CẤU HÌNH DỊCH VỤ AUTHENTICATION
// ==========================================
builder.Services.AddAuthentication(options =>
{
    // Thiết lập mặc định: Dùng Cookie để lưu phiên, dùng Google để đăng nhập
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie() // Thêm Cookie
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});

// Add session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ==========================================
// 3. KÍCH HOẠT MIDDLEWARE AUTHENTICATION
// ==========================================
app.UseAuthentication(); // <-- Quan trọng: Phải đặt TRƯỚC UseAuthorization

app.UseAuthorization();

app.UseSession();

// Area routing must come before default routing
app.MapControllerRoute(
    name: "admin",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.Run();