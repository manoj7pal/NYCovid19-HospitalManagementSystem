using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CovidDashboard.Controllers
{
    public class PatientController : Controller
    {
        Data.CovidDashboardDBContext _context;

        public PatientController(Data.CovidDashboardDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Call to get the list of Patients from the DB. It depends of hospital id passed.
        /// </summary>
        /// <param name="hid">Hospital Id</param>
        /// <returns>Model having Patients list to the View</returns>
        [Route("/Patient/Index/{hid?}")]
        public IActionResult Index(int hid = 0)
        {
            var model = new Models.PatientHomeModel(_context.Patients.ToList());
            if (hid != 0)
            {
                model = new Models.PatientHomeModel(_context.Patients.Where(t => t.HospitalId == hid).ToList());
                var hospital = _context.Hospitals.Where(t => t.Id == hid).FirstOrDefault();
                ViewBag.HospitalName = hospital.Name;
            }
            return View(model);
        }


        /// <summary>
        /// It will Create/Update the Patient as per the requirement. If there is Id present then it will update otherwise create
        /// </summary>
        /// <param name="id">Patient Id</param>
        /// <returns>Model having Patient to Create / Update</returns>
        [Route("/Patient/Manage/{id?}")]
        public IActionResult Manage(int id = 0)
        {
            var model = new Models.ManagePatientModel();
            model.RequestedId = id;

            if (id.Equals(0)) // check for new Patient
            {
                model.NewPatient = true;
                model.Patient = new Models.Patient();

            }
            else
            {
                var patient = _context.Patients.Where(r => r.Id.Equals(id)).FirstOrDefault();

                if (patient != null)
                {
                    var hospital = _context.Hospitals.Where(t => t.Id == patient.HospitalId).FirstOrDefault();
                    model.Patient = new Models.Patient(patient);
                    model.Patient.Hospital = new Models.Hospital() { Id = hospital.Id, Name = hospital.Name };
                }
            }

            var hospitalList = _context.Hospitals.OrderBy(x => x.Id).ToList();
            model.Patient.Hospitals = new List<Models.Hospital>();
            foreach (var item in hospitalList)
            {
                model.Patient.Hospitals.Add(new Models.Hospital() { Id = item.Id, Name = item.Name });
            }

            return View(model);
        }

        /// <summary>
        /// Saves the data as per the Patient Model to the DB
        /// </summary>
        /// <param name="patient">Model Patient</param>
        /// <returns>Redirect to the Index Action</returns>
        [HttpPost]
        public IActionResult SavePatient(Models.Patient patient)
        {
            if (patient.Id > 0)
            {
                var data = _context.Patients.Where(r => r.Id.Equals(patient.Id)).FirstOrDefault();

                if (data == null)
                {
                    throw new Exception("patient not found");
                }

                // data.Name = patient.Name;
                data.Address = patient.Address;
                data.ContactNumber = patient.ContactNumber;
                data.IsDischarged = patient.IsDischarged;
                // data.DateOfAdmission = patient.DateOfAdmission;
                if (data.IsDischarged && data.DateOfDischarge.Date == DateTime.Parse("0001-01-01"))
                {
                    data.DateOfDischarge = DateTime.Now;
                }
                data.HospitalId = patient.Hospital.Id;
                data.Age = patient.Age;
            }
            else
            {
                _context.Patients.Add(new Data.Entities.Patient()
                {
                    Name = patient.Name,
                    Address = patient.Address,
                    ContactNumber = patient.ContactNumber,
                    IsDischarged = patient.IsDischarged,
                    DateOfAdmission = DateTime.Now,
                    DateOfDischarge = patient.DateOfDischarge,
                    HospitalId = patient.Hospital.Id,
                    Age = patient.Age
                });
            }
            _context.SaveChanges();

            return RedirectToAction("Index", "Patient", new { hid = patient.Hospital.Id });
        }

        /// <summary>
        /// Delete the Patient selected
        /// </summary>
        /// <param name="id">Patient Id</param>
        /// <returns>Json true or false</returns>
        [HttpPost]
        public IActionResult RemovePatient(int id)
        {
            try
            {
                var patient = _context.Patients.Where(r => r.Id.Equals(id)).FirstOrDefault(); // selects patient if exists as per the Id
                var hospitalId = patient.HospitalId;
                if (patient == null)
                    throw new Exception("no Patient found");

                _context.Patients.Remove(patient);
                _context.SaveChanges();

                return RedirectToAction("Index", "Patient", new { hid = hospitalId });
            }
            catch (Exception e)
            {
                return new JsonResult(false);
            }
        }
    }
}
