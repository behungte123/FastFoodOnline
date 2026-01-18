# 🍔 FastFoodOnline

Dự án web quản lý và bán đồ ăn nhanh trực tuyến.  
Cho phép người dùng đăng ký, đăng nhập, đặt món và quản trị sản phẩm.

---

## 🚀 Chức năng chính

- Đăng ký / Đăng nhập tài khoản  
- Đăng nhập bằng Google  
- Quản lý sản phẩm (CRUD)  
- Đặt hàng online  
- Gửi email xác nhận

---

## 🛠️ Công nghệ sử dụng

- ASP.NET Core MVC (.NET 8)  
- SQL Server  
- Entity Framework Core  
- Identity Authentication  
- Bootstrap + jQuery

---

## ⚙️ Cài đặt & chạy

### 1. Clone source
git clone https://github.com/beuhungte123/FastFoodOnline.git

csharp
Sao chép mã

### 2. Cấu hình database  
Sửa file `appsettings.json`:

"ConnectionStrings": {
"DefaultConnection": "Server=YOUR_SERVER;Database=FastFoodDB;Trusted_Connection=True;TrustServerCertificate=True"
}

shell
Sao chép mã

### 3. Chạy migration
dotnet ef database update

shell
Sao chép mã

### 4. Run project
dotnet run

css
Sao chép mã

Truy cập:
https://localhost:xxxx

yaml
Sao chép mã

---

## 🔐 Lưu ý cấu hình

File `appsettings.json` cần tự điền:

- Connection String SQL Server  
- Email SMTP  
- Google ClientId & ClientSecret  

(Không push giá trị thật lên GitHub)

---

## 👨‍💻 Tác giả

- Hưng – FPT Polytechnic  
- Môn: Lập trình C# / ASP.NET Core

---

## ⭐ Ghi chú

Dự án phục vụ mục đích học tập.