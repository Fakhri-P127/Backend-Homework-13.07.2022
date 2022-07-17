using Backend_Homework_Pronia.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_Homework_Pronia.Models
{
    public class Setting:BaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }

    }
}
