using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KisaanMart.Data;
using KisaanMart.Data.Models;
using System.Collections.Specialized;
using System.Net;

namespace KisaanMart.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ApplicationContext _context;

        public TransactionsController(ApplicationContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult> RenderImage(int id)
        {
            Machine item = await _context.Machines.FindAsync(id);

            byte[] photoBack = item.MachineImage;

            //return File(photoBack, "image/png");
            return Json(new { base64imgage = Convert.ToBase64String(photoBack)});
        }
        public IActionResult LoadTransactionsForCurrentUser()
        {
            try
            {
                var user = getCurrentUser();
                var transactions = _context.Transactions.Include(t => t.UserMachines).Where(t => t.requesteduserId == user.Id).Select(t=>new {
                  StartDate=t.StartDate.ToShortDateString(),
                    EndDate=t.EndDate.ToShortDateString(),
                    Noofhours=t.NoOfHours,
                    Noofacres=t.NoOfAcres,
                    Totalamount=t.TotalAmountToBePaid
                }).ToList();
               
                //Returning Json Data  
                return Json(new { data = transactions });
                //return Json(new { data = data });

            }
            catch (Exception)
            {
                throw;
            }

        }

        public User getCurrentUser()
        {
            User user = _context.Users.FirstOrDefault<User>(m => m.UserPhoneNo == User.Identity.Name);
            return user;
        }
        [HttpGet]
        public JsonResult GetModeratorUsers()
        {
            User currentUser = getCurrentUser();

            var query = (from c in _context.Users
                         where c.ModeratorId==currentUser.Id
                         select new SelectListItem()
                         {
                             Value = c.Id.ToString(),
                             Text = c.UserName+" - "+c.UserPhoneNo
                         });
            return new JsonResult(query.ToList());
        }
        [HttpGet]
        public IActionResult GetCurrentUser()
        {

            var user = getCurrentUser();
            return Json(new { data = user });
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var user = getCurrentUser();
            var applicationContext = _context.Transactions.Include(t => t.UserMachines).Where(t=>t.requesteduserId==user.Id);
            return View(await applicationContext.ToListAsync());
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.UserMachines)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            ViewData["UserMachineId"] = new SelectList(_context.UserMachines, "Id", "Id");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartDate,EndDate,NoOfHours,NoOfAcres,TotalAmountToBePaid,UserMachineId,requesteduserId,behalfuserId,Purpose")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                if (User.IsInRole("Moderator"))
                {
                    var user = getCurrentUser();
                    var moderator = await _context.Users.FindAsync(user.Id);
                    moderator.PointsAccumulated += 5;
                    _context.Update(moderator);
                }
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                SendSMSToUser(transaction);
                SendSMSToOwner(transaction);
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserMachineId"] = new SelectList(_context.UserMachines, "Id", "Id", transaction.UserMachineId);
            return View(transaction);
        }

        public void SendSMSToUser(Transaction transaction)
        {
            var userMachineDetails = _context.UserMachines.Find(transaction.UserMachineId);
            User user,owner;
            if(User.IsInRole("Moderator")&&transaction.behalfuserId!=null)
            {
                user = _context.Users.Find(transaction.behalfuserId);
                
            }
            else
            {
                user = _context.Users.Find(transaction.requesteduserId);
                
            }
            owner = _context.Users.Find(userMachineDetails.userId);

            var MachineDetails = _context.Machines.Find(userMachineDetails.MachineId);


            var message = "Hi " + user.UserName + " Your request is confirmed for Machine:" + MachineDetails.MachineName + " Owner:" + owner.UserName + " Start Date:" + transaction.StartDate.ToShortDateString() + " Amount:" + transaction.TotalAmountToBePaid; 



            var phonnum = "91" + user.UserPhoneNo;
            NameValueCollection nvc = new NameValueCollection() {
                {"apikey" , "JYMcLr0pf6Q-3WsLiX9LCuK9gufjmsHCCzNo7J0AIU"},
                {"numbers" , phonnum},
                {"message" , message},
                {"sender" , "TXTLCL"}
            };
            using (var wb = new WebClient())
            {
                byte[] response = wb.UploadValues("https://api.textlocal.in/send/", nvc);
                //string result = System.Text.Encoding.UTF8.GetString(response);

            }

        }
        public void SendSMSToOwner(Transaction transaction)
        {
            var userMachineDetails = _context.UserMachines.Find(transaction.UserMachineId);
            User user, owner;
            if (User.IsInRole("Moderator") && transaction.behalfuserId != null)
            {
                user = _context.Users.Find(transaction.behalfuserId);

            }
            else
            {
                user = _context.Users.Find(transaction.requesteduserId);

            }
            owner = _context.Users.Find(userMachineDetails.userId);

            var MachineDetails = _context.Machines.Find(userMachineDetails.MachineId);


            var message = "Hi " + owner.UserName + " New order for Machine:" + MachineDetails.MachineName + " Requestor:" + user.UserName + " Start Date:" + transaction.StartDate.ToShortDateString() + " Amount:" + transaction.TotalAmountToBePaid;



            var phonnum = "91" + owner.UserPhoneNo;
            NameValueCollection nvc = new NameValueCollection() {
                {"apikey" , "JYMcLr0pf6Q-3WsLiX9LCuK9gufjmsHCCzNo7J0AIU"},
                {"numbers" , phonnum},
                {"message" , message},
                {"sender" , "TXTLCL"}
            };
            using (var wb = new WebClient())
            {
                byte[] response = wb.UploadValues("https://api.textlocal.in/send/", nvc);
                //string result = System.Text.Encoding.UTF8.GetString(response);

            }

        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["UserMachineId"] = new SelectList(_context.UserMachines, "Id", "Id", transaction.UserMachineId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartDate,EndDate,NoOfHours,NoOfAcres,TotalAmountToBePaid,UserMachineId,requesteduserId,behalfuserId,Purpose")] Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.Id))
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
            ViewData["UserMachineId"] = new SelectList(_context.UserMachines, "Id", "Id", transaction.UserMachineId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.UserMachines)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }
    }
}
