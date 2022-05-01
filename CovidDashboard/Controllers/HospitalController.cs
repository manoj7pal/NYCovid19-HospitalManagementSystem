using CovidDashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CovidDashboard.Controllers
{
    public class HospitalController : Controller
    {
        Data.CovidDashboardDBContext _context;

        public HospitalController(Data.CovidDashboardDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Hospital List with the data required to show up 
        /// </summary>
        /// <returns>Model containing Hospital data</returns>
        public IActionResult Index()
        {
            var model = new Models.HospitalHomeModel(_context.Hospitals.Include("Patients").ToList());
            foreach (var item in model.Hospitals)
            {
                
                var regionSelected = _context.Regions.Where(t => t.Id == item.RegionId).FirstOrDefault();
                item.Region = new Region(){ Id = regionSelected.Id, Name = regionSelected.Name };
            }            

            return View(model);
        }

        /// <summary>
        /// Create/Update the hospital depending on the Id provided. If Id is present then Update otherwise Add
        /// </summary>
        /// <param name="id">Hospital Id</param>
        /// <returns>Model having hospital details to the View</returns>

        [Route("/Hospital/Manage/{id?}")]
        public IActionResult Manage(int id = 0)
        {
            var model = new Models.ManageHospitalModel();
            model.RequestedId = id;

            if (id.Equals(0))
            {
                model.NewHospital = true;
                model.Hospital = new Hospital();
            }
            else
            {
                var hospital = _context.Hospitals.Include("Patients").ToList().Where(r => r.Id.Equals(id)).FirstOrDefault();

                if (hospital != null)
                {
                    model.Hospital = new Models.Hospital(hospital);
                }
            }

            var regionsList = _context.Regions.ToList();
            model.Hospital.Regions = new List<Region>();
            foreach (var region in regionsList)
            {
                model.Hospital.Regions.Add(new Models.Region { Id = region.Id, Name = region.Name });
            }

            return View(model);
        }

        /// <summary>
        /// Saves Hospital Data to the DB
        /// </summary>
        /// <param name="hospital">Hospital Model to save the Data to DB</param>
        /// <returns>Redirects to the Index of Hospital Controller</returns>
        [HttpPost]
        public IActionResult SaveHospital(Models.Hospital hospital)
        {
            if (hospital.Id > 0)
            {
                var data = _context.Hospitals.Where(r => r.Id.Equals(hospital.Id)).FirstOrDefault();

                if (data == null)
                {
                    throw new Exception("Hospital not found");
                }

                data.RegionId = hospital.Region.Id;
            }
            else
            {
                _context.Hospitals.Add(new Data.Entities.Hospital()
                {
                    Name = hospital.Name,
                    RegionId = hospital.Region.Id
                });
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Hospital", null);
        }

        /// <summary>
        /// Delete the Hospital as per the id provided
        /// </summary>
        /// <param name="id">Hospital Id</param>
        /// <returns>JSon True or False</returns>
        [HttpPost]
        public IActionResult RemoveHospital(int id)
        {
            try
            {
                var hospital = _context.Hospitals.Where(r => r.Id.Equals(id)).FirstOrDefault();

                if (hospital == null)
                    throw new Exception("no hospital found");

                _context.Hospitals.Remove(hospital);
                _context.SaveChanges();

                return new JsonResult(true);
            }
            catch (Exception e)
            {
                return new JsonResult(false);
            }
        }
    }
}
