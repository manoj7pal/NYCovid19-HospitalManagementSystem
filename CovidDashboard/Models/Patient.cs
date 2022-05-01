using System;
using System.Collections.Generic;

namespace CovidDashboard.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public int Age { get; set; }

        public bool IsDischarged { get; set; }
        public DateTime DateOfAdmission { get; set; }
        public DateTime DateOfDischarge { get; set; }

        public Hospital Hospital { get; set; }

        public List<Hospital> Hospitals { get; set; }

        public Patient(Data.Entities.Patient patient)
        {
            Id = patient.Id;
            Name = patient.Name;
            Address = patient.Address;
            ContactNumber = patient.ContactNumber;
            IsDischarged = patient.IsDischarged;
            DateOfAdmission = patient.DateOfAdmission;
            DateOfDischarge = patient.DateOfDischarge;
            patient.Hospital = patient.Hospital;
            Age = patient.Age;
            
        }

        public Patient()
        {

        }
    }
}
