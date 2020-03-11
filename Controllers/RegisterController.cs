using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KisaanMart.Data;
using KisaanMart.Data.Models;
using KisaanMart.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Specialized;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace KisaanMart.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ApplicationContext _context;
        UtilityHelper util = new UtilityHelper();
        public RegisterController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Register
        
        public async Task<IActionResult> Index()
        {
            
            return View(await _context.TempUsers.ToListAsync());
        }

        // GET: Register/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tempUser = await _context.TempUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tempUser == null)
            {
                return NotFound();
            }

            return View(tempUser);
        }

        // GET: Register/Create
        public IActionResult Create(string Register)
        {
            TempData.Remove("UserType");
            if (Register== "REGISTER AS EXPERT"||Register== "REGISTER AS COMMUNITY VOLUNTEER")
            {
                TempData["UserType"] = "Moderator";
            }
            else
            {
                TempData["UserType"] = "User";
            }
            return View();
        }

        // POST: Register/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,UserPhoneNo,UserPassword,IsModerator,Latitude,Longitude,RandOTP")] TempUser tempUser)
        {
            var currentuser = _context.Users.FirstOrDefault(n => n.UserPhoneNo == tempUser.UserPhoneNo);
            if (currentuser!=null)
            {
                ModelState.AddModelError("UserPhoneNo", "Phone number already registered.Login to your account");
                return View(tempUser);
            }

            if (TempData["UserType"].ToString() == "Moderator")
            {
                tempUser.IsModerator = true;

            }
            else
            {
                tempUser.IsModerator = false;
            }
            var randOTP = util.GenerateRandomOTP(4, new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" });
            tempUser.RandOTP = randOTP;
            if (ModelState.IsValid)
            {                            
                _context.Add(tempUser);
                await _context.SaveChangesAsync();
                SendOTPToUser(tempUser.UserPhoneNo,tempUser.RandOTP);
                return RedirectToAction("Edit","Register",new { id=tempUser.Id});
            }
            //var query = from state in ModelState.Values
            //            from error in state.Errors
            //            select error.ErrorMessage;
            //var errors = query.ToArray();
            return View(tempUser);
        }
        public void  SendOTPToUser(string userPhoneNo,int? userOTP)
        {
            var phonnum = "91" + userPhoneNo;
            NameValueCollection nvc = new NameValueCollection() {
                {"apikey" , "JYMcLr0pf6Q-3WsLiX9LCuK9gufjmsHCCzNo7J0AIU"},
                {"numbers" , phonnum},
                {"message" , "Thank you for registering with Kisaan App. Your OTP is "+userOTP},
                {"sender" , "TXTLCL"}
            };
            using (var wb = new WebClient())
            {
                byte[] response = wb.UploadValues("https://api.textlocal.in/send/", nvc);
                //string result = System.Text.Encoding.UTF8.GetString(response);

            }

        }
        // GET: Register/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            TempData.Remove("Success");
            TempData["Success"] = "Enter OTP to complete registration and login";
            if (id == null)
            {
                return NotFound();
            }

            var tempUser = await _context.TempUsers.FindAsync(id);
            if (tempUser == null)
            {
                return NotFound();
            }
            return View(tempUser);
        }

        // POST: Register/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,UserPhoneNo,UserPassword,IsModerator,Latitude,Longitude,RandOTP")] TempUser tempUser)
        {
            if (id != tempUser.Id)
            {
                return NotFound();
            }
            var currentuser = _context.TempUsers.FirstOrDefault(n => n.UserPhoneNo == tempUser.UserPhoneNo);
            
            if (currentuser.RandOTP != tempUser.RandOTP)
            {
                ModelState.AddModelError("RandOTP", "Enter valid OTP");
                return View(tempUser);
            }
            _context.Entry(currentuser).State = EntityState.Detached;

            if (ModelState.IsValid)
            {
                try
                {
                    
                    User user = new User()
                    {
                        UserPhoneNo = tempUser.UserPhoneNo,                        
                        IsActive = true,
                        UserName=tempUser.UserName,
                        Latitude=tempUser.Latitude,
                        Longitude=tempUser.Longitude,                        
                        Role = currentuser.IsModerator == true ? "Moderator" : "User"
                    };
                    _context.Add(user);
                    _context.Remove(tempUser);
                    await _context.SaveChangesAsync();

                    //var user = _context.Users.FirstOrDefault(n => n.UserPhoneNo == loginViewModel.UserPhoneNo);

                    //Check the user name and password  
                    //Here can be implemented checking logic from the database  

                    if (user != null)
                    {
                        //loginViewModel.Role = user.Role;
                        var claims = new[] { new Claim(ClaimTypes.Name, user.UserPhoneNo), new Claim(ClaimTypes.Role, user.Role) };

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


                        var principal = new ClaimsPrincipal(identity);

                        var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("UserPhoneNo", "Enter valid username or password");

                        return View();
                    }
                }
                catch (Exception e)
                {
                    var exception = e.Message;
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;
                    var errors = query.ToArray();
                }

            }

            return View(tempUser);
        }

        // GET: Register/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tempUser = await _context.TempUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tempUser == null)
            {
                return NotFound();
            }

            return View(tempUser);
        }

        // POST: Register/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tempUser = await _context.TempUsers.FindAsync(id);
            _context.TempUsers.Remove(tempUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TempUserExists(int id)
        {
            return _context.TempUsers.Any(e => e.Id == id);
        }
    }
}
