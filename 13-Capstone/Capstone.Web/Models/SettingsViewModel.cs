using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Web.Models
{
    public class SettingsViewModel
    {
        [Required]
        [Display(Name = "Temperature Setting")]
        public string TemperatureSetting { get; set; }
        public List<SelectListItem> Choices
        {
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem("Fahrenheit", "fahrenheit"),
                    new SelectListItem("Celsius", "celsius")
                };
            }
        }
    }
}
