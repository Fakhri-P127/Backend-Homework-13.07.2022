using Backend_Homework_Pronia.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_Homework_Pronia.Models
{
    public class PlantSize:BaseEntity
    {
        public int PlantId { get; set; }
        public Plant Plant { get; set; }
        public int SizeId { get; set; }
        public Size Size { get; set; }

    }
}
