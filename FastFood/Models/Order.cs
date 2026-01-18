using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastFoodOnline.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Display(Name = "Khách hàng")]
        public int AccountId { get; set; }
        public Account? Account { get; set; }

        [Display(Name = "Ngày đặt")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Display(Name = "Địa chỉ giao hàng")]
        [StringLength(255)]
        public string? DeliveryAddress { get; set; }

        [Display(Name = "Trạng thái")]
        [StringLength(50)]
        public string Status { get; set; } = "Chưa giao";

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Tổng tiền")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(500)]
        public string? Notes { get; set; }

        public ICollection<OrderDetail> Items { get; set; } = new List<OrderDetail>();
    }
}
