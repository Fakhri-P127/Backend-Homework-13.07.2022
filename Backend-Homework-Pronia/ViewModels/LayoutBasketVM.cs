using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_Homework_Pronia.ViewModels
{
    public class LayoutBasketVM
    {

        public List<BasketItemVM> BasketItemVMs { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
