
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CovidDashboard.Data.Entities
{
    public class Hospital
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int RegionId { get; set; }

        public List<Patient> Patients { get; set; }
    }
}
