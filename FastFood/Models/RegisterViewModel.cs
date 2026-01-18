using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FastFoodOnline.Models
{
    public class RegisterViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Họ tên không được bỏ trống")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Họ tên phải từ 3-100 ký tự")]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được bỏ trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Mật khẩu phải từ 8-100 ký tự")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{8,}$",
            ErrorMessage = "Mật khẩu phải có ít nhất 1 chữ hoa, 1 chữ thường, 1 số và 1 ký tự đặc biệt")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Xác nhận mật khẩu không được bỏ trống")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        [Display(Name = "Xác nhận mật khẩu")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại không được bỏ trống")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [RegularExpression(@"^0[0-9]{9}$", ErrorMessage = "Số điện thoại phải là 10 số và bắt đầu bằng 0")]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Địa chỉ không được bỏ trống")]
        [StringLength(255, MinimumLength = 10, ErrorMessage = "Địa chỉ phải từ 10-255 ký tự")]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ngày sinh không được bỏ trống")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh")]
        public DateTime DateOfBirth { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Check age (must be at least 13 years old)
            if (DateOfBirth != default)
            {
                var age = DateTime.Today.Year - DateOfBirth.Year;
                if (DateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;

                if (age < 13)
                {
                    yield return new ValidationResult(
                        "Bạn phải từ 13 tuổi trở lên để đăng ký",
                        new[] { nameof(DateOfBirth) });
                }

                if (DateOfBirth > DateTime.Today)
                {
                    yield return new ValidationResult(
                        "Ngày sinh không được là ngày trong tương lai",
                        new[] { nameof(DateOfBirth) });
                }
            }
        }
    }
}
