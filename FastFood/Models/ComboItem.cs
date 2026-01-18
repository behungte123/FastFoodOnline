using System.ComponentModel.DataAnnotations;

namespace FastFoodOnline.Models
{
    public class ComboItem
    {
        public int Id { get; set; }

        [Display(Name = "Combo")]
        public int ComboId { get; set; }

        [Display(Name = "Món ăn")]
        public int FoodId { get; set; }

        [Range(1, 20)]
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        public Combo? Combo { get; set; }
        public Food? Food { get; set; }
    }
}
