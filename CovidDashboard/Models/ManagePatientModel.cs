namespace CovidDashboard.Models
{
    public class ManagePatientModel
    {
        public int RequestedId { get; set; }
        public bool NewPatient { get; set; }
        public bool PatientFound { get { return Patient != null || NewPatient; } }
        public Patient Patient { get; set; }
    }
}
