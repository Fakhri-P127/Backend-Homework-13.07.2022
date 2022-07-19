using Backend_Homework_Pronia.Models.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_Homework_Pronia.Models
{
    public class Plant:BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Desc { get; set; }
        public string SKU { get; set; }
        public List<PlantImage> PlantImages { get; set; }
        public int PlantInformationId { get; set; }
        public PlantInformation PlantInformation { get; set; }
        public List<PlantCategory> PlantCategories { get; set; }
        public List<PlantColor> PlantColors { get; set; }
        public List<PlantSize> PlantSizes { get; set; }
        public List<PlantTag> PlantTags { get; set; }
        [NotMapped]
        public IFormFile MainPhoto { get; set; }
        [NotMapped]

        public IFormFile HoverPhoto { get; set; }
        [NotMapped]

        public List<IFormFile> DetailPhotos { get; set; }
        [NotMapped]
        public List<int> CategoryIds { get; set; }
    }
}
