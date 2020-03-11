using System;
using System.Collections.Generic;
using System.Linq;
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
    public class AppsController : Controller
    {
        private readonly ApplicationContext _context;
        [HttpGet]
        public IActionResult LoadAppData()
        {
            try
            {
               
                // getting all Customer data  
                var apps = (from app in _context.Apps
                                select app);
                
                //Returning Json Data  
                return Json(new { data = apps });
                //return Json(new { data = data });

            }
            catch (Exception)
            {
                throw;
            }

        }
        public AppsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Apps
        public async Task<IActionResult> Index()
        {
            return View(await _context.Apps.ToListAsync());
        }

        // GET: Apps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apps = await _context.Apps
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apps == null)
            {
                return NotFound();
            }

            return View(apps);
        }

        // GET: Apps/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Apps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AppName,AppDescription,AppUrl,AppImage")] Apps apps, [FromForm(Name = "AppImage")] IFormFile AppImage)
        {
            if (ModelState.IsValid)
            {

                if (AppImage != null && AppImage.Length > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = AppImage.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                    apps.AppImage = p1;
                }
                _context.Add(apps);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var query = from state in ModelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;
            var errors = query.ToArray();
            return View(apps);
        }

        // GET: Apps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apps = await _context.Apps.FindAsync(id);
            if (apps == null)
            {
                return NotFound();
            }
            return View(apps);
        }

        // POST: Apps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AppName,AppDescription,AppUrl,AppImage")] Apps apps)
        {
            if (id != apps.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(apps);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppsExists(apps.Id))
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
            return View(apps);
        }

        // GET: Apps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apps = await _context.Apps
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apps == null)
            {
                return NotFound();
            }

            return View(apps);
        }

        // POST: Apps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var apps = await _context.Apps.FindAsync(id);
            _context.Apps.Remove(apps);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppsExists(int id)
        {
            return _context.Apps.Any(e => e.Id == id);
        }
    }
}
