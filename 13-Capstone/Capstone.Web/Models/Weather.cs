using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Web.Models
{
    public class Weather
    {
        public string ParkCode { get; set; }
        public int FiveDayForecast { get; set; }
        public double LowTemp { get; set; }
        public double HighTemp { get; set; }
        public string Forecast { get; set; }

        public string ForecastMessage
        {
            get
            {
                string output = "";

                if(Forecast == "snow")
                {
                    output = "Pack snowshoes!";
                }
                else if (Forecast == "rain")
                {
                    output = "Pack rain gear and wear waterproof shoes!";
                }
                else if (Forecast == "thunderstorms")
                {
                    output = "Seek shelter and avoid hiking on exposed ridges!";
                }
                else if (Forecast == "sunny")
                {
                    output = "Pack sunblock!";
                }

                return output;
            }
        }

        public string TemperatureMessage
        {
            get
            {
                string output = "";

                if(HighTemp > 75)
                {
                    output = "Pack an extra gallon of water! ";
                }
                if(HighTemp - LowTemp > 20)
                {
                    output += "Wear breathable layers! ";
                }
                if(LowTemp < 20)
                {
                    output += "Be careful and dress warm!";
                }

                return output;
            }
        }

        public string DayOfWeek
        {
            get
            {
                string output = "";
                
                if((int)DateTime.Now.DayOfWeek + (FiveDayForecast - 1) > 6)
                {
                    output = (DateTime.Now.DayOfWeek + (FiveDayForecast - 8)).ToString();
                }
                else
                {
                    output = (DateTime.Now.DayOfWeek + (FiveDayForecast - 1)).ToString();
                }

                return output;
            }
        }
    }
}
