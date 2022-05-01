using CovidDashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CovidDashboard.Controllers
{
    public class HomeController : Controller
    {
        Data.CovidDashboardDBContext _context;

        private readonly ILogger<HomeController> _logger;
        public HomeController(Data.CovidDashboardDBContext context)
        {
            _context = context;
        }

        
        /// <summary>
        /// This is landing Action of the Application. Whenever application run this is the starting point.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var region = _context.Regions.Count();
            if (region == 0)
            {
                this.FetchRegionsFromAPI();
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// This is API call to get the Region List from the  health data NY gov API to the DB table
        /// </summary>
        public void FetchRegionsFromAPI()
        {
            var apiPath = string.Empty;
            string API_KEY = "";
            apiPath = "https://health.data.ny.gov/resource/jw46-jpb7.json"; // NY Gov API URL
            var jsonInputData = string.Empty;

            HttpClient httpClient;
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", API_KEY);
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            httpClient.BaseAddress = new Uri(apiPath);
            try
            {
                HttpResponseMessage response = httpClient.GetAsync(apiPath)
                                                       .GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    jsonInputData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }

                if (jsonInputData != string.Empty)
                {
                    // JsonConvert is part of the NewtonSoft.Json Nuget package
                    var data = JsonConvert.DeserializeObject<List<DataModel_cs>>(jsonInputData).ToList();

                    var regionList = data.ToList().Select(t => t.ny_forward_region).Distinct();
                    int i = 0;
                    foreach (var region in regionList)
                    {
                        i = i + 1;
                        _context.Regions.Add(new Data.Entities.Region()
                        {
                            Name = region                           
                        });
                    }
                    _context.SaveChanges(); // Regions getting saved to the DB Region table

                }
            }
            catch (Exception ex)
            {

            }
        }       
    }
}
