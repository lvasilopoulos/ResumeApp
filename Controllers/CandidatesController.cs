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
    public class CandidatesController : Controller
    {
        private readonly AppDbContext _context;

        public CandidatesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Candidates
        public async Task<IActionResult> Index()
        {
            var candidates = await _context.Candidates
                                   .Include(c => c.Degrees) // Eager loading
                                   .ToListAsync();
            //return View(await _context.Candidates.ToListAsync());
            return View(candidates);
        }

        // GET: Candidates/Create
        public async Task<IActionResult> CreateAsync()
        {
            var degrees = await _context.Degrees.ToListAsync();
            if (degrees.Count == 0)
            {
                ViewData["Degrees"] = null;
            }
            else 
            {
                ViewData["Degrees"] = new SelectList(degrees, "Id", "Name");
            }
            
            return View();
        }

        // POST: Candidates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LastName,FirstName,Email,Mobile,DegreeId,")] Candidates candidates)
        {
            if (ModelState.IsValid)
            {
                candidates.CreationTime = DateTime.Now;

                _context.Add(candidates);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var degrees = await _context.Degrees.ToListAsync();        
            ViewData["Degrees"] = new SelectList(degrees, "Id", "Name");

            return View(candidates);
        }

        // GET: Candidates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var degrees = await _context.Degrees.ToListAsync();
            var candidates = await _context.Candidates.FindAsync(id);
            if (candidates == null)
            {
                return NotFound();
            }
            ViewData["Degrees"] = new SelectList(degrees, "Id", "Name");
            return View(candidates);
        }

        // POST: Candidates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LastName,FirstName,Email,Mobile,DegreeId,")] Candidates candidates)
        {
            if (id != candidates.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {                  
                    _context.Update(candidates);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CandidatesExists(candidates.Id))
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
            return View(candidates);
        }

        // GET: Candidates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidates = await _context.Candidates
                .Include(c => c.Degrees)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (candidates == null)
            {
                return NotFound();
            }

            return View(candidates);
        }

        // POST: Candidates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var candidates = await _context.Candidates.FindAsync(id);
            if (candidates != null)
            {
                _context.Candidates.Remove(candidates);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CandidatesExists(int id)
        {
            return _context.Candidates.Any(e => e.Id == id);
        }
    }
}
