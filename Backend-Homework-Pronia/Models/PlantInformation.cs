using Backend_Homework_Pronia.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_Homework_Pronia.Models
{
    public class PlantInformation:BaseEntity
    {
        public string Shipping { get; set; }
        public string AboutReturnRequest { get; set; }
        public string Guarentee { get; set; }
        public List<Plant> Plants { get; set; }
        public string Definition { get; set; }

    }
}
