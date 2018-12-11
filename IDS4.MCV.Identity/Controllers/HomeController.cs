using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IDS4.MCV.Identity.Models;

namespace IDS4.MCV.Identity.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            string note = System.DateTime.Now.ToShortDateString() + " © Accede Cloud911.Heimdall";
            return Content (note);
        }

       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
