using System;
using System.Collections.Generic;
using System.Linq;

namespace CovidDashboard.Models
{
    public class Hospital
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int RegionId { get; set; }
        public Region Region { get; set; }

        public int AdmittedPatients { get; set; }
        public int NewlyAdmittedPatients { get; set; }
        public int DischargedPatients { get; set; }

        public List<Region> Regions { get; set; }

        public Hospital(Data.Entities.Hospital hospital)
        {
            Id = hospital.Id;
            Name = hospital.Name;
            RegionId = hospital.RegionId;

            AdmittedPatients = hospital.Patients.Where(r => r.IsDischarged == false).Count();
            NewlyAdmittedPatients = hospital.Patients.Where(r => r.IsDischarged == false && r.DateOfAdmission.Date == DateTime.Today).Count();
            DischargedPatients = hospital.Patients.Where(r => r.IsDischarged == true).Count();
        }

        public Hospital()
        {

        }
    }
}
