using Backend_Homework_Pronia.Models.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_Homework_Pronia.Models
{
    public class Slider:BaseEntity
    {
        public string Image { get; set; }
        [Required]
        public string Discount { get; set; }
        [Required]

        public string Title { get; set; }
        [Required(ErrorMessage ="Please input the title.")]

        public string Article { get; set; }
        [Required]

        public string ButtonUrl { get; set; }
        [Required]

        public byte Order { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }

    }
}
