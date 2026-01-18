using System.Collections.Generic;

namespace FastFoodOnline.Models
{
    public class HomeViewModel
    {
        public List<Food> PopularFoods { get; set; } = new();
        public List<Combo> Combos { get; set; } = new();
    }
}
