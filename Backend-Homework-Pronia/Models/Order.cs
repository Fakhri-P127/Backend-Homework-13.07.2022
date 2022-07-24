using Backend_Homework_Pronia.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_Homework_Pronia.Models
{
    public class Order:BaseEntity
    {
        public decimal TotalPrice { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public string Address { get; set; }
        public bool? Status { get; set; }
        public List<BasketItem> BasketItems { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

    }
}
