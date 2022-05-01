namespace CovidDashboard.Models
{
    public class ManageHospitalModel
    {
        public int RequestedId { get; set; }
        public bool NewHospital { get; set; }
        public bool HospitalFound { get { return Hospital != null || NewHospital; } }
        public Hospital Hospital { get; set; }
    }
}
