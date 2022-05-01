using Microsoft.EntityFrameworkCore;
using CovidDashboard.Data.Entities;

namespace CovidDashboard.Data
{
    public class CovidDashboardDBContext : DbContext
    {
        public DbSet<Region> Regions { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Patient> Patients { get; set; }
       

        public CovidDashboardDBContext(DbContextOptions options) : base(options)
        {

        }
    }
}
