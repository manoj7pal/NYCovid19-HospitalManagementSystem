using System.Collections.Generic;

namespace CovidDashboard.Models
{
    public class HospitalHomeModel
    {
        public List<Hospital> Hospitals { get; set; }

        public HospitalHomeModel(List<Data.Entities.Hospital> hospitals)
        {
            Hospitals = new List<Hospital>();

            foreach (var hospital in hospitals)
            {
                Hospitals.Add(new Hospital(hospital));
            }
        }
    }
}
