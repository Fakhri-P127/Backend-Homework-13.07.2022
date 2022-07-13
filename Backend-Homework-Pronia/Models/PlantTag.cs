using Backend_Homework_Pronia.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_Homework_Pronia.Models
{
    public class PlantTag:BaseEntity
    {
        public int PlantId { get; set; }
        public Plant Plant { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }

    }
}
