using Microsoft.AspNetCore.Mvc;

namespace CovidDashboard.Controllers
{
    public class About : Controller
    {
        /// <summary>
        /// It will redirects to the About Us Page
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}
