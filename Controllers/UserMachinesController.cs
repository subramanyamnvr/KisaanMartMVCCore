using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KisaanMart.Data;
using KisaanMart.Data.Models;

namespace KisaanMart.Controllers
{
    public class UserMachinesController : Controller
    {
        private readonly ApplicationContext _context;

        public UserMachinesController(ApplicationContext context)
        {
            _context = context;
        }

        public User getCurrentUser()
        {
            User user = _context.Users.FirstOrDefault<User>(m => m.UserPhoneNo == User.Identity.Name);
            return user;
        }

        // GET: UserMachines
        public async Task<IActionResult> Index()
        {
            User currentUser = getCurrentUser();
            var applicationContext = _context.UserMachines.Include(u => u.Machines).Include(u => u.user).Where(u=>u.user.UserPhoneNo==currentUser.UserPhoneNo);
            return View(await applicationContext.ToListAsync());
        }
        public IActionResult GetUserMachineDetails(int usermachineId)
        {

            var query = (from usermachine in _context.UserMachines
                         where usermachine.Id== usermachineId
                         select usermachine);
            return  Json(new { data= query });
        }
        public IActionResult LoadDataForSelectedMachine(int selectedMachineId,string StartDate,string EndDate)
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();

                // Skip number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();

                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();

                // Sort Column Name  
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

                // Sort Column Direction (asc, desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                // Search Value from (Search box)  
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10, 20, 50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;

                int skip = start != null ? Convert.ToInt32(start) : 0;

                int recordsTotal = 0;

                DateTime startdate = Convert.ToDateTime(StartDate);
                DateTime enddate = Convert.ToDateTime(EndDate);
                //&& m.StartDate > startdate && m.StartDate < startdate && m.EndDate > enddate && m.EndDate < enddate
                //var machines= _context.Transactions.Include(t=>t.UserMachines).Include(t=>t.UserMachines.Machines).Where(m => m.UserMachines.MachineId == selectedMachineId).Select(t => new
                //{
                //    MachineId = t.UserMachines.Id,
                //    AmountPerHour = t.UserMachines.AmountPerHour,
                //    AmountPerAcre = t.UserMachines.AmountPerAcre,
                //    MachineName = t.UserMachines.Machines.MachineName,
                //    UserPhoneNo = t.UserMachines.user.UserPhoneNo,
                //    Distance = t.UserMachines.Distance
                //}).ToList();
                var machines = _context.UserMachines.Where(m => m.Machines.Id == selectedMachineId).OrderBy(t => t.Distance).Select(t => new
                {
                    MachineId = t.Id,
                    AmountPerHour = t.AmountPerHour,
                    AmountPerAcre = t.AmountPerAcre,
                    MachineName = t.Machines.MachineName,
                    UserPhoneNo = t.user.UserPhoneNo,
                    Distance = t.Distance
                }).ToList();
                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //machines = machines.OrderBy(sortColumn + " " + sortColumnDirection);
                    //machines= (sortColumnDirection == "ASC") ? machines.OrderBy(sortColumn).AsQueryable() : machines.OrderByDescending(sortColumn).AsQueryable()
                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                 //   machines = machines.Where(m => m.MachineName.Contains(searchValue));
                }

                //total number of rows counts   
                recordsTotal = machines.Count();
                //Paging   
                var data = machines.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                //return Json(new { data = data });

            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        public IActionResult LoadMachinesForCurrentUser()
        {
            try
            {
                User currentUser = getCurrentUser(); 
                
                // getting all Customer data  
                //var machines = (from machine in _context.Machines select machine); Include(u => u.Machines).Include(u => u.user).
                var machines = _context.UserMachines.Include(u=>u.user).Where(m => m.user.Id == currentUser.Id).Select(t => new
                {
                    MachineId = t.Id,
                    AmountPerHour = t.AmountPerHour,
                    AmountPerAcre = t.AmountPerAcre,
                    MachineName = t.Machines.MachineName,
                    UserPhoneNo = t.user.UserPhoneNo,
                    MachineImage= t.Machines.MachineImage
                }).ToList();              
                //Returning Json Data  
                return Json(new { data = machines });
                //return Json(new { data = data });

            }
            catch (Exception)
            {
                throw;
            }

        }
        // GET: UserMachines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userMachine = await _context.UserMachines
                .Include(u => u.Machines)
                .Include(u => u.user)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userMachine == null)
            {
                return NotFound();
            }

            return View(userMachine);
        }

        // GET: UserMachines/Create
        public IActionResult Create()
        {
            ViewData["MachineId"] = new SelectList(_context.Machines, "Id", "MachineName");
            //ViewData["userId"] = new SelectList(_context.Users, "Id", "UserPhoneNo");
            return View();
        }

        
        // POST: UserMachines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AmountPerHour,AmountPerAcre,userId,MachineId")] UserMachine userMachine)
        {
            if(userMachine.MachineId==0)
            {
                ModelState.AddModelError("Machines", "Select valid machine");
            }

            if (ModelState.IsValid)
            {
                Random rnd = new Random();
                userMachine.user = getCurrentUser();
                userMachine.Distance = rnd.Next(1, 20);
                _context.Add(userMachine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MachineId"] = new SelectList(_context.Machines, "Id", "MachineName", userMachine.MachineId);
            ViewData["userId"] = new SelectList(_context.Users, "Id", "UserPhoneNo", userMachine.userId);
            return View(userMachine);
        }

        // GET: UserMachines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userMachine = await _context.UserMachines.FindAsync(id);
            if (userMachine == null)
            {
                return NotFound();
            }
            ViewData["MachineId"] = new SelectList(_context.Machines, "Id", "Id", userMachine.MachineId);
            ViewData["userId"] = new SelectList(_context.Users, "Id", "UserPhoneNo", userMachine.userId);
            return View(userMachine);
        }

        // POST: UserMachines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AmountPerHour,AmountPerAcre,userId,MachineId")] UserMachine userMachine)
        {
            if (id != userMachine.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var Machine = await _context.UserMachines.FindAsync(id);
                    userMachine.Distance = Machine.Distance;
                    _context.Update(userMachine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserMachineExists(userMachine.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MachineId"] = new SelectList(_context.Machines, "Id", "Id", userMachine.MachineId);
            ViewData["userId"] = new SelectList(_context.Users, "Id", "UserPhoneNo", userMachine.userId);
            return View(userMachine);
        }

        // GET: UserMachines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userMachine = await _context.UserMachines
                .Include(u => u.Machines)
                .Include(u => u.user)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userMachine == null)
            {
                return NotFound();
            }

            return View(userMachine);
        }

        // POST: UserMachines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userMachine = await _context.UserMachines.FindAsync(id);
            _context.UserMachines.Remove(userMachine);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserMachineExists(int id)
        {
            return _context.UserMachines.Any(e => e.Id == id);
        }
    }
}
