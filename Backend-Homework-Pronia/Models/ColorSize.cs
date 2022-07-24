using Backend_Homework_Pronia.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_Homework_Pronia.Models
{
    public class ColorSize:BaseEntity
    {
        public int ColorId { get; set; }
        public Color Color { get; set; }
        public int SizeId { get; set; }
        public Size Size { get; set; }
    }
}
