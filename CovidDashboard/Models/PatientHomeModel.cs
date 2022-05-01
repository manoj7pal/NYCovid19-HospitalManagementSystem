using System.Collections.Generic;

namespace CovidDashboard.Models
{
    public class PatientHomeModel
    {
        public List<Patient> Patients { get; set; }

        public PatientHomeModel(List<Data.Entities.Patient> patients)
        {
            Patients = new List<Patient>();

            foreach (var patient in patients)
            {
                Patients.Add(new Patient(patient));
            }
        }
    }
}
