using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastFoodOnline.Models
{
    public class Food
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Tên món ăn")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Range(0, 1000000)]
        [Display(Name = "Đơn giá")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [StringLength(255)]
        [Display(Name = "Ảnh minh họa")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Loại món")]
        public int CategoryId { get; set; }
        public FoodCategory? Category { get; set; }

        [StringLength(200)]
        [Display(Name = "Từ khóa / Chủ đề")]
        public string? Tag { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<ComboItem> ComboItems { get; set; } = new List<ComboItem>();
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
