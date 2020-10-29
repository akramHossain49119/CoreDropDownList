using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreDropDownList.Data;
using CoreDropDownList.Models;

namespace CoreDropDownList.Controllers
{
    public class CenterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CenterController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Center
        public async Task<IActionResult> Index()
        {
            return View(await _context.Centers.ToListAsync());
        }

        // GET: Center/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var center = await _context.Centers
                .FirstOrDefaultAsync(m => m.Id == id); 
            if (center == null)
            {
                return NotFound();
            }

            return View(center);
        }

        // GET: Center/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Center/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CenterId,CenterName,CenterCode")] Center center)
        {
            if (ModelState.IsValid)
            {
                _context.Add(center);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(center);
        }

        // GET: Center/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var center = await _context.Centers.FindAsync(id);
            if (center == null)
            {
                return NotFound();
            }
            return View(center);
        }

        // POST: Center/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CenterName,CenterCode")] Center center)
        {
            if (id != center.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(center);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CenterExists(center.Id)) 
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
            return View(center);
        }

        // GET: Center/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var center = await _context.Centers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (center == null)
            {
                return NotFound();
            }

            return View(center);
        }

        // POST: Center/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var center = await _context.Centers.FindAsync(id);
            _context.Centers.Remove(center);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CenterExists(int id)
        {
            return _context.Centers.Any(e => e.Id == id);
        }
    }
}
