using System;
using System.ComponentModel.DataAnnotations;

namespace FastFoodOnline.Models
{
    public class Account
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Họ tên không được bỏ trống")]
        [StringLength(100)]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được bỏ trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 ký tự trở lên")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; } = string.Empty;

        [Phone]
        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }

        [StringLength(255)]
        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh")]
        public DateTime? DateOfBirth { get; set; }

        public int RoleId { get; set; }
        public Role? Role { get; set; }

        // ==========================================
        // CÁC TRƯỜNG MỚI THÊM ĐỂ XÁC THỰC EMAIL
        // ==========================================

        // Trạng thái kích hoạt: true = Đã kích hoạt, false = Chưa kích hoạt
        public bool IsActive { get; set; } = false;

        // Mã Token gửi qua email (Dùng cho đăng ký thường)
        // Cho phép null vì đăng nhập Google không cần cái này
        public string? ActivationToken { get; set; }
    }
}