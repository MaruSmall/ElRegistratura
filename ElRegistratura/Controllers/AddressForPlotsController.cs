using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElRegistratura.Data;
using ElRegistratura.Models;

namespace ElRegistratura.Controllers
{
    public class AddressForPlotsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AddressForPlotsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AddressForPlots
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AddressForPlot.Include(a => a.Plot).Include(a => a.Street);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AddressForPlots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var addressForPlot = await _context.AddressForPlot
                .Include(a => a.Plot)
                .Include(a => a.Street)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (addressForPlot == null)
            {
                return NotFound();
            }

            return View(addressForPlot);
        }

        // GET: AddressForPlots/Create
        public IActionResult Create()
        {
            ViewData["PlotId"] = new SelectList(_context.Plots, "Id", "Name");
            ViewData["StreetId"] = new SelectList(_context.Street, "Id", "Name");
            return View();
        }

        // POST: AddressForPlots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,HouseNumber,StreetId,PlotId")] AddressForPlot addressForPlot)
        {
            if (ModelState.IsValid)
            {
                _context.Add(addressForPlot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlotId"] = new SelectList(_context.Plots, "Id", "Name", addressForPlot.PlotId);
            ViewData["StreetId"] = new SelectList(_context.Street, "Id", "Name", addressForPlot.StreetId);
            return View(addressForPlot);
        }

        // GET: AddressForPlots/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var addressForPlot = await _context.AddressForPlot.FindAsync(id);
            if (addressForPlot == null)
            {
                return NotFound();
            }
            ViewData["PlotId"] = new SelectList(_context.Plots, "Id", "Name", addressForPlot.PlotId);
            ViewData["StreetId"] = new SelectList(_context.Street, "Id", "Name", addressForPlot.StreetId);
            return View(addressForPlot);
        }

        // POST: AddressForPlots/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,HouseNumber,StreetId,PlotId")] AddressForPlot addressForPlot)
        {
            if (id != addressForPlot.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(addressForPlot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressForPlotExists(addressForPlot.Id))
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
            ViewData["PlotId"] = new SelectList(_context.Plots, "Id", "Name", addressForPlot.PlotId);
            ViewData["StreetId"] = new SelectList(_context.Street, "Id", "Name", addressForPlot.StreetId);
            return View(addressForPlot);
        }

        // GET: AddressForPlots/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var addressForPlot = await _context.AddressForPlot
                .Include(a => a.Plot)
                .Include(a => a.Street)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (addressForPlot == null)
            {
                return NotFound();
            }

            return View(addressForPlot);
        }

        // POST: AddressForPlots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var addressForPlot = await _context.AddressForPlot.FindAsync(id);
            _context.AddressForPlot.Remove(addressForPlot);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AddressForPlotExists(int id)
        {
            return _context.AddressForPlot.Any(e => e.Id == id);
        }
    }
}
