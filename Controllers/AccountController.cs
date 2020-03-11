using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KisaanMart.Data;
using KisaanMart.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using KisaanMart.Utility;
using System.Collections.Specialized;
using System.Net;

namespace KisaanMart.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationContext _context;
        UtilityHelper util = new UtilityHelper();

        public AccountController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Account
        public async Task<IActionResult> Index()
        {
            return View(await _context.LoginViewModel.ToListAsync());
        }

               
        public IActionResult Login()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Login([Bind("Id,UserPhoneNo,UserPassword,Role,OTP")] LoginViewModel loginViewModel)
        {
             var user = _context.Users.FirstOrDefault(n => n.UserPhoneNo == loginViewModel.UserPhoneNo);

            //Check the user name and password  
            //Here can be implemented checking logic from the database  
            var randOTP = util.GenerateRandomOTP(4, new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" });
            if (user != null)
            {
                user.RandOTP = randOTP;
                _context.Update(user);
                _context.SaveChanges();
                SendOTPToUser(user.UserName,user.UserPhoneNo, user.RandOTP);

                return RedirectToAction("OTPScreen", "Account", loginViewModel);
            }
            else
            {
                ModelState.AddModelError("UserPhoneNo", "Enter valid username or password");
                
                return View(loginViewModel);
            }

            //if (ModelState.IsValid)
            //{
            //    _context.Add(loginViewModel);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            
        }
        public void SendOTPToUser(string UserName,string userPhoneNo, int? userOTP)
        {
            var phonnum = "91" + userPhoneNo;
            NameValueCollection nvc = new NameValueCollection() {
                {"apikey" , "JYMcLr0pf6Q-3WsLiX9LCuK9gufjmsHCCzNo7J0AIU"},
                {"numbers" , phonnum},
                {"message" , "Hi "+UserName +" Your OTP to login to Kisaanvaradhi is "+userOTP},
                {"sender" , "TXTLCL"}
            };
            using (var wb = new WebClient())
            {
                byte[] response = wb.UploadValues("https://api.textlocal.in/send/", nvc);
                //string result = System.Text.Encoding.UTF8.GetString(response);

            }

        }
        public IActionResult OTPScreen([Bind("Id,UserPhoneNo,UserPassword,Role,OTP")] LoginViewModel loginViewModel)
        {
            var user = _context.Users.FirstOrDefault(n => n.UserPhoneNo == loginViewModel.UserPhoneNo && n.RandOTP==loginViewModel.OTP);
            if (user != null)
            {
                loginViewModel.Role = user.Role;
                var claims = new[] { new Claim(ClaimTypes.Name, user.UserPhoneNo), new Claim(ClaimTypes.Role, user.Role) };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


                var principal = new ClaimsPrincipal(identity);

                var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                //ModelState.AddModelError("UserPhoneNo", "Enter valid username/OTP");

                return View(loginViewModel);
            }
        }

              
          
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }



        private bool LoginViewModelExists(int id)
        {
            return _context.LoginViewModel.Any(e => e.Id == id);
        }
    }
}
