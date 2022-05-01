using System.Collections.Generic;

namespace CovidDashboard.Models
{
    public class Chart
    {
        public string[] labels { get; set; }
        public List<Datasets> datasets { get; set; }
    }
}
