using Backend_Homework_Pronia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_Homework_Pronia.ViewModels
{
    public class BasketVM
    {
        public List<BasketCookieItemVM> BasketCookieItemVMs { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
