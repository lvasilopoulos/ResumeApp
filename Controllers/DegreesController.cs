using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ResumeApp.Data;
using ResumeApp.Models;

namespace ResumeApp.Controllers
{
    public class DegreesController : Controller
    {
        private readonly AppDbContext _context;

        public DegreesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Degree/Cleanup
        public async Task<IActionResult> Cleanup()
        {
            // Find degrees that are not associated with any candidates
            var degreesWithoutCandidates = await _context.Degrees
                .Where(d => !_context.Candidates.Any(c => c.DegreeId == d.Id))
                .ToListAsync();

            return View(degreesWithoutCandidates);
        }

        // POST: Degree/Cleanup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CleanupConfirmed()
        {
            // Find degrees that are not associated with any candidates
            var degreesWithoutCandidates = await _context.Degrees
                .Where(d => !_context.Candidates.Any(c => c.DegreeId == d.Id))
                .ToListAsync();

            _context.Degrees.RemoveRange(degreesWithoutCandidates);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Degrees
        public async Task<IActionResult> Index()
        {
            return View(await _context.Degrees.ToListAsync());
        }

        // GET: Degrees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Degrees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Degrees degrees)
        {
            if (ModelState.IsValid)
            {
                _context.Add(degrees);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(degrees);
        }

        // GET: Degrees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var degrees = await _context.Degrees.FindAsync(id);
            if (degrees == null)
            {
                return NotFound();
            }
            return View(degrees);
        }

        // POST: Degrees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Degrees degrees)
        {
            if (id != degrees.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(degrees);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DegreesExists(degrees.Id))
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
            return View(degrees);
        }

        // GET: Degrees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var degrees = await _context.Degrees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (degrees == null)
            {
                return NotFound();
            }

            return View(degrees);
        }

        // POST: Degrees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var degrees = await _context.Degrees.FindAsync(id);
            if (degrees != null)
            {
                _context.Degrees.Remove(degrees);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DegreesExists(int id)
        {
            return _context.Degrees.Any(e => e.Id == id);
        }
    }
}
