using Course_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.Controllers
{
    public class HomeGuestController : Controller
    {
        private readonly ILogger<HomeGuestController> _logger;

       // private ApplicationDbContext db;

        public HomeGuestController(
            ILogger<HomeGuestController> logger)
        {
            _logger = logger;
            //db = context;
        }

        public IActionResult Index()
        {
            //var reviews = db.Reviews.ToList();


            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "HomeAuthorized");
            return View();
        }

        public IActionResult Reviews()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
