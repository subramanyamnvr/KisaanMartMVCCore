using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KisaanMart.Models;
using KisaanMart.Data.Models;
using KisaanMart.Data;
using Microsoft.AspNetCore.Http;

namespace KisaanMart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationContext _context;

        public HomeController(ApplicationContext context)
        {
            _context = context;
        }
        public User getCurrentUser()
        {
            User user = _context.Users.FirstOrDefault<User>(m => m.UserPhoneNo == User.Identity.Name);
            return user;
        }
        public IActionResult Index()
        {
            var currentUser = getCurrentUser();
            if(currentUser!=null)
            { 
            HttpContext.Session.SetString("AccumulatedPoints", currentUser.PointsAccumulated.ToString());
            HttpContext.Session.SetString("UserName", currentUser.UserName.ToString());
            HttpContext.Session.SetString("UserPhoneNo", currentUser.UserPhoneNo.ToString());
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult PolyhouseFarming()
        {
            return View();
        }

        public IActionResult CropInsurance()
        {
            return View();
        }
        public IActionResult Advisory()
        {
            return View();
        }

        public IActionResult ProcessingUnit()
        {
            return View();
        }
        public IActionResult Fertilizer()
        {
            return View();
        }
        public IActionResult SeedTesting()
        {
            return View();
        }
        public IActionResult AgroMarketing()
        {
            return View();
        }
        public IActionResult StorageUnit()
        {
            return View();
        }
        public IActionResult SoilTesting()
        {
            return View();
        }
        public IActionResult OrganicFarming()
        {
            return View();
        }



    }
}
