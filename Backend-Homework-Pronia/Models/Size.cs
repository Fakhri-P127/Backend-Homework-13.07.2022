using Backend_Homework_Pronia.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_Homework_Pronia.Models
{
    public class Size:BaseEntity
    {
        public string Name { get; set; }
        public List<PlantSize> PlantSizes { get; set; }

    }
}
