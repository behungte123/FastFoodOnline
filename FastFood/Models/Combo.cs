using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastFoodOnline.Models
{
    public class Combo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Tên combo")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Range(0, 1000000)]
        [Display(Name = "Giá combo")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [StringLength(255)]
        [Display(Name = "Ảnh minh họa")]
        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<ComboItem> Items { get; set; } = new List<ComboItem>();
    }
}
