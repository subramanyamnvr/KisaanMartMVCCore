using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KisaanMart.Data;
using KisaanMart.Data.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace KisaanMart.Controllers
{
    public class MachinesController : Controller
    {
        private readonly ApplicationContext _context;

        public MachinesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Machines
        public async Task<IActionResult> Index()
        {
            return View(await _context.Machines.ToListAsync());
        }
        public JsonResult GetMachines()
        {
            
            var query = (from c in _context.Machines
                         select new SelectListItem()
                         {
                             Value = c.Id.ToString(),
                             Text = c.MachineName
                         });
            return new JsonResult(query.ToList());
        }

        public IActionResult LoadMachineData()
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

                // getting all Customer data  
                var machines = (from machine in _context.Machines
                                    select machine);
                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //machines = machines.OrderBy(sortColumn + " " + sortColumnDirection);
                    //machines= (sortColumnDirection == "ASC") ? machines.OrderBy(sortColumn).AsQueryable() : machines.OrderByDescending(sortColumn).AsQueryable()
                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    machines = machines.Where(m => m.MachineName.Contains(searchValue));
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
        // GET: Machines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var machine = await _context.Machines
                .FirstOrDefaultAsync(m => m.Id == id);
            if (machine == null)
            {
                return NotFound();
            }

            return View(machine);
        }

        // GET: Machines/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Machines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MachineName,MachineDescription,MachineImage")] Machine machine, [FromForm(Name = "MachineImage")] IFormFile MachineImage)
        {
            if (ModelState.IsValid)
            {
                if (MachineImage != null && MachineImage.Length > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = MachineImage.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                    machine.MachineImage = p1;
                }
                _context.Add(machine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(machine);
        }
        public async Task<ActionResult> RenderImage(int id)
        {
            Machine item = await _context.Machines.FindAsync(id);

            byte[] photoBack = item.MachineImage;

            return File(photoBack, "image/png");
        }
        // GET: Machines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var machine = await _context.Machines.FindAsync(id);
            if (machine == null)
            {
                return NotFound();
            }
            return View(machine);
        }

        // POST: Machines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MachineName,MachineDescription")] Machine machine)
        {
            if (id != machine.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //machine.MachineImage = Convert.ToBase64String(MachineImage);
                    _context.Update(machine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MachineExists(machine.Id))
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
            return View(machine);
        }

        // GET: Machines/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var machine = await _context.Machines
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (machine == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(machine);
        //}

        // POST: Machines/Delete/5
        [HttpPost]
        
        public async Task<IActionResult> Delete(int id)
        {
            var machine = await _context.Machines.FindAsync(id);
            _context.Machines.Remove(machine);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MachineExists(int id)
        {
            return _context.Machines.Any(e => e.Id == id);
        }
    }
}
