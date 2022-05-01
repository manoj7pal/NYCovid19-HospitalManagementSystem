using Microsoft.AspNetCore.Mvc;
using ChartJSCore.Helpers;
using ChartJSCore.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using CovidDashboard.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CovidDashboard.Controllers
{
    public class ChartController : Controller
    {
        private readonly IConfiguration _config;
        public List<string> regionList = new List<string>();
        public List<double> admissionReportedList = new List<double>();

        Data.CovidDashboardDBContext _context;

        public ChartController(IConfiguration config, Data.CovidDashboardDBContext context)
        {
            _config = config;
            _context = context;
        }

        public IActionResult Index()
        {
            ChartJSCore.Models.Chart chart = new ChartJSCore.Models.Chart();

            ChartJSCore.Models.Chart lineChart = GenerateLineChart();
            ViewData["LineChart"] = lineChart;

            return View();
        }

        public IActionResult Chart()
        {
            ChartJSCore.Models.Chart pieChart = GeneratePieChart();
            ViewData["PieChart"] = pieChart;

            return View();
        }

        private ChartJSCore.Models.Chart GenerateLineChart()
        {
            this.FetchRegionsFromAPI();
            ChartJSCore.Models.Chart chart = new ChartJSCore.Models.Chart();
            chart.Type = Enums.ChartType.Line;

            ChartJSCore.Models.Data data = new ChartJSCore.Models.Data();
            data.Labels = new List<string>();
            data.Labels = regionList;

            LineDataset dataset = new LineDataset()
            {
                Label = "NY Covid Dashboard",
                Data = admissionReportedList,
                Fill = "false",
                LineTension = 0.1,
                BackgroundColor = ChartColor.FromRgba(75, 192, 192, 0.4),
                BorderColor = ChartColor.FromRgba(75, 192, 192, 1),
                BorderCapStyle = "butt",
                BorderDash = new List<int> { },
                BorderDashOffset = 0.0,
                BorderJoinStyle = "miter",
                PointBorderColor = new List<ChartColor>() { ChartColor.FromRgba(75, 192, 192, 1) },
                PointBackgroundColor = new List<ChartColor>() { ChartColor.FromHexString("#fff") },
                PointBorderWidth = new List<int> { 1 },
                PointHoverRadius = new List<int> { 5 },
                PointHoverBackgroundColor = new List<ChartColor>() { ChartColor.FromRgba(75, 192, 192, 1) },
                PointHoverBorderColor = new List<ChartColor>() { ChartColor.FromRgba(220, 220, 220, 1) },
                PointHoverBorderWidth = new List<int> { 2 },
                PointRadius = new List<int> { 1 },
                PointHitRadius = new List<int> { 10 },
                SpanGaps = false
            };

            data.Datasets = new List<Dataset>();
            data.Datasets.Add(dataset);

            Options options = new Options()
            {
                Scales = new Scales()
            };

            Scales scales = new Scales()
            {
                YAxes = new List<Scale>()
                {
                    new CartesianScale()
                }
            };

            CartesianScale yAxes = new CartesianScale()
            {
                Ticks = new Tick()
            };

            Tick tick = new Tick()
            {
                Callback = "function(value, index, values) {return value;}"
            };

            yAxes.Ticks = tick;
            scales.YAxes = new List<Scale>() { yAxes };
            options.Scales = scales;
            chart.Options = options;

            chart.Data = data;

            return chart;
        }


        private ChartJSCore.Models.Chart GeneratePieChart()
        {
            var model = new Models.HospitalHomeModel(_context.Hospitals.Include("Patients").ToList());
            double admittedPatients = 0.0;
            double newlyAdmittedPatients = 0.0;
            double dischargedPatients = 0.0;

            foreach (var item in model.Hospitals)
            {
                admittedPatients = admittedPatients + item.AdmittedPatients;
                newlyAdmittedPatients = newlyAdmittedPatients + item.NewlyAdmittedPatients;
                dischargedPatients = dischargedPatients + item.DischargedPatients;
            }

            ChartJSCore.Models.Chart chart = new ChartJSCore.Models.Chart();
            chart.Type = Enums.ChartType.Pie;

            ChartJSCore.Models.Data data = new ChartJSCore.Models.Data();
            data.Labels = new List<string>() { "Patients Admitted", "Patients Discharged", "Newly Admitted Patients" };

            PieDataset dataset = new PieDataset()
            {
                Label = "My dataset",
                BackgroundColor = new List<ChartColor>() {
                    ChartColor.FromHexString("#FF6384"),
                    ChartColor.FromHexString("#36A2EB"),
                    ChartColor.FromHexString("#FFCE56")
                },
                HoverBackgroundColor = new List<ChartColor>() {
                    ChartColor.FromHexString("#FF6384"),
                    ChartColor.FromHexString("#36A2EB"),
                    ChartColor.FromHexString("#FFCE56")
                },
                Data = new List<double>() { admittedPatients, dischargedPatients, newlyAdmittedPatients }
            };

            data.Datasets = new List<Dataset>();
            data.Datasets.Add(dataset);

            chart.Data = data;

            return chart;
        }


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

                    regionList = data.ToList().Select(t => t.ny_forward_region).Distinct().ToList();

                    foreach (var item in regionList)
                    {
                        var numinRegion = data.ToList().Where(t => t.ny_forward_region == item).Select(t => t.cumulative_covid_19_discharges);
                        var totalCount = 0.0;
                        foreach (var num in numinRegion)
                        {
                            totalCount = totalCount + Convert.ToDouble(num);
                        }

                        if (totalCount != null)
                        {
                            admissionReportedList.Add(Convert.ToDouble(totalCount));
                        }
                        else
                        {
                            admissionReportedList.Add(0);
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }


    }
}
