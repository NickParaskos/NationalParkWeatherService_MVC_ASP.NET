using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Web.Models
{
    public class DetailsViewModel
    {
        public Park Park { get; set; }
        public List<Weather> FiveDayWeather { get; set; }
        public string TemperatureSetting { get; set; }
    }
}
