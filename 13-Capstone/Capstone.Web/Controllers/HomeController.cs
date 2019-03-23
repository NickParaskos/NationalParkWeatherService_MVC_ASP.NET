using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Capstone.Web.Models;
using Capstone.Web.DAL.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Capstone.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly string TEMPERATURE_SETTING_SESSION_KEY = "Temperature_Setting";
        private readonly IParkSqlDAL parkDAL;
        private readonly ISurvey_ResultSqlDAL survey_ResultDAL;
        private readonly IWeatherSqlDAL weatherDAL;

        public HomeController(IParkSqlDAL parkDAL, ISurvey_ResultSqlDAL survey_ResultDAL, IWeatherSqlDAL weatherDAL)
        {
            this.parkDAL = parkDAL;
            this.survey_ResultDAL = survey_ResultDAL;
            this.weatherDAL = weatherDAL;
        }


        public IActionResult Index()
        {
            List<Park> parks = parkDAL.GetAllParks();

            return View(parks);
        }

        public IActionResult Details(string data)
        {
            DetailsViewModel model = new DetailsViewModel();
            model.Park = parkDAL.GetPark(data);
            model.FiveDayWeather = weatherDAL.GetWeatherForPark(data);
            model.TemperatureSetting = GetCurrentTemperatureSetting();
            return View(model);
        }

        [HttpGet]
        public IActionResult Survey()
        {
            List<Park> parks = parkDAL.GetAllParks();
            ViewBag.ParkSelectList = parkDAL.GetParkSelectList();
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Survey(Survey_Result surveyResult)
        {
            surveyResult.State = surveyResult.State.ToUpper();
            survey_ResultDAL.AddResult(surveyResult);

            return RedirectToAction("SurveyResults");
        }

        public IActionResult SurveyResults()
        {
            List<SurveyResultViewModel> surveys = new List<SurveyResultViewModel>();

            surveys = survey_ResultDAL.GetTopRankedParks();

            return View(surveys);
        }

        [HttpGet]
        public IActionResult Settings()
        {
            SettingsViewModel model = new SettingsViewModel();
            model.TemperatureSetting = GetCurrentTemperatureSetting();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Settings(SettingsViewModel model)
        {
            SaveCurrentTemperatureSetting(model.TemperatureSetting);
            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string GetCurrentTemperatureSetting()
        {
            string setting = HttpContext.Session.GetString(TEMPERATURE_SETTING_SESSION_KEY);

            if (string.IsNullOrWhiteSpace(setting))
            {
                setting = "fahrenheit";
                SaveCurrentTemperatureSetting(setting);
            }

            return setting;
        }

        private void SaveCurrentTemperatureSetting(string setting)
        {
            HttpContext.Session.SetString(TEMPERATURE_SETTING_SESSION_KEY, setting);
        }
    }
}
