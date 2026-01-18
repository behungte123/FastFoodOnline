using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FastFoodOnline.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Description { get; set; }

        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
