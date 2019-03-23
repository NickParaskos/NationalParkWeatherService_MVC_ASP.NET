using Capstone.Web.DAL;
using Capstone.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;

namespace CapstoneTests.DAL_Tests
{
    [TestClass]
    public class WeatherSqlDALTests
    {
        private TransactionScope tran;
        private string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=NPGeek;Integrated Security=True";

        [TestInitialize]
        public void TestInitialize()
        {
            tran = new TransactionScope();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd;
                conn.Open();

                cmd = new SqlCommand("INSERT INTO park (parkCode, parkName, state, acreage, elevationInFeet, milesOfTrail, numberOfCampsites, climate, yearFounded, annualVisitorCount, inspirationalQuote, inspirationalQuoteSource, parkDescription, entryFee, numberOfAnimalSpecies) VALUES ('ABC', 'Test Park National Park', 'Ohio', 100, 1, 10, 0, 'Woodland', 2019, 7, 'To say something inspirational you must be inspired.', 'Nick Paraskos', 'Park Description', 0, 500)", conn);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("INSERT INTO weather (parkCode, fiveDayForecastValue, low, high, forecast) VALUES ('ABC', 1, 38, 62, 'rain')", conn);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("INSERT INTO weather (parkCode, fiveDayForecastValue, low, high, forecast) VALUES ('ABC', 2, 38, 56, 'sunny')", conn);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("INSERT INTO weather (parkCode, fiveDayForecastValue, low, high, forecast) VALUES ('ABC', 3, 53, 66, 'partly cloudy')", conn);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("INSERT INTO weather (parkCode, fiveDayForecastValue, low, high, forecast) VALUES ('ABC', 4, 70, 82, 'thunderstorms')", conn);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("INSERT INTO weather (parkCode, fiveDayForecastValue, low, high, forecast) VALUES ('ABC', 5, 71, 88, 'rain')", conn);
                cmd.ExecuteNonQuery();
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            tran.Dispose();
        }

        [TestMethod()]
        public void GetWeatherTest()
        {
            WeatherSqlDAL weatherSqlDAL = new WeatherSqlDAL(connectionString);

            List<Weather> weathers = weatherSqlDAL.GetWeather();

            int result = 0;
            foreach (Weather weather in weathers)
            {
                if (weather.ParkCode == "ABC")
                {
                    result++;
                }
            }

            Assert.AreEqual(5, result);
        }

        [TestMethod()]
        public void GetWeatherForParkTest()
        {
            WeatherSqlDAL weatherSqlDAL = new WeatherSqlDAL(connectionString);

            List<Weather> weatherForPark = weatherSqlDAL.GetWeatherForPark("ABC");

            Assert.AreEqual(5, weatherForPark.Count);
            Assert.AreEqual("rain", weatherForPark[0].Forecast);
            Assert.AreEqual(5, weatherForPark[4].FiveDayForecast);
            Assert.AreEqual(70, weatherForPark[3].LowTemp);
        }
    }
}
