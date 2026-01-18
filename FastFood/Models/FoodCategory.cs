using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FastFoodOnline.Models
{
    public class FoodCategory
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Tên loại món")]
        public string Name { get; set; } = string.Empty;

        [StringLength(255)]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        public ICollection<Food> Foods { get; set; } = new List<Food>();
    }
}
