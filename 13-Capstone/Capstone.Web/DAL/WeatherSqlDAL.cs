using Capstone.Web.DAL.Interfaces;
using Capstone.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Web.DAL
{
    public class WeatherSqlDAL : IWeatherSqlDAL
    {
        private const string SQL_GetWeather = "SELECT * FROM weather";
        private const string SQL_GetWeatherForPark = "SELECT * FROM weather where parkCode = @parkCode ORDER BY fiveDayForecastValue";

        private string connectionString;

        public WeatherSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Weather> GetWeather()
        {
            List<Weather> output = new List<Weather>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetWeather, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Weather weather = new Weather();

                        weather.ParkCode = Convert.ToString(reader["parkCode"]);
                        weather.FiveDayForecast = Convert.ToInt32(reader["fiveDayForecastValue"]);
                        weather.LowTemp = Convert.ToInt32(reader["low"]);
                        weather.HighTemp = Convert.ToInt32(reader["high"]);
                        weather.Forecast = Convert.ToString(reader["forecast"]);


                        output.Add(weather);
                    }

                }
            }
            catch
            {
                output = new List<Weather>();
            }

            return output;
        }

        public List<Weather> GetWeatherForPark(string parkCode)
        {
            List<Weather> output = new List<Weather>();

            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL_GetWeatherForPark, conn);
                cmd.Parameters.AddWithValue("@parkCode", parkCode);

                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    Weather w = new Weather()
                    {
                        ParkCode = Convert.ToString(reader["parkCode"]),
                        FiveDayForecast = Convert.ToInt32(reader["fiveDayForecastValue"]),
                        LowTemp = Convert.ToDouble(reader["low"]),
                        HighTemp = Convert.ToDouble(reader["high"]),
                        Forecast = Convert.ToString(reader["forecast"])
                    };

                    if (w.Forecast == "partly cloudy")
                    {
                        w.Forecast = "partlyCloudy";
                    }

                    output.Add(w);
                }
            }

            return output;
        }
    }
}
