using System;
using System.ComponentModel.DataAnnotations;
namespace CovidDashboard.Data.Entities
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }

        public bool IsDischarged { get; set; }
        public DateTime DateOfAdmission { get; set; }
        public DateTime DateOfDischarge { get; set; }

        public int HospitalId { get; set; }

        public int Age { get; set; }

        public Hospital Hospital { get; set; }
    }
}
