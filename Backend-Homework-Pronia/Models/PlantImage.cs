using Backend_Homework_Pronia.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_Homework_Pronia.Models
{
    public class PlantImage:BaseEntity
    {
        public string Name { get; set; }
        public string Alternative { get; set; }
        public bool? IsMain { get; set; }
        public int PlantId { get; set; }
        public Plant Plant { get; set; }
        //[NotMapped]
        //public List<int> ImageIds { get; set; }
        //[NotMapped]
        //public string ImageName { get; set; }

    }
}
