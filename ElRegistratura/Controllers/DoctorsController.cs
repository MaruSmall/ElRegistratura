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
    public class DoctorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Doctors
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Doctors.Include(d => d.Category).Include(d => d.Clinic).Include(d => d.Plot).Include(d => d.Position).Include(d => d.Speciality);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include(d => d.Category)
                .Include(d => d.Clinic)
                .Include(d => d.Plot)
                .Include(d => d.Position)
                .Include(d => d.Speciality)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["ClinicId"] = new SelectList(_context.Clinics, "Id", "HouseNumb");
            ViewData["PlotId"] = new SelectList(_context.Plots, "Id", "Id");
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Id");
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Id");
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LastName,FirstName,Patromynic,DoctorPicture,ClinicId,CategoryId,PositionId,SpecialityId,PlotId")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", doctor.CategoryId);
            ViewData["ClinicId"] = new SelectList(_context.Clinics, "Id", "HouseNumb", doctor.ClinicId);
            ViewData["PlotId"] = new SelectList(_context.Plots, "Id", "Id", doctor.PlotId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Id", doctor.PositionId);
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Id", doctor.SpecialityId);
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", doctor.CategoryId);
            ViewData["ClinicId"] = new SelectList(_context.Clinics, "Id", "HouseNumb", doctor.ClinicId);
            ViewData["PlotId"] = new SelectList(_context.Plots, "Id", "Id", doctor.PlotId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Id", doctor.PositionId);
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Id", doctor.SpecialityId);
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LastName,FirstName,Patromynic,DoctorPicture,ClinicId,CategoryId,PositionId,SpecialityId,PlotId")] Doctor doctor)
        {
            if (id != doctor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(doctor.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", doctor.CategoryId);
            ViewData["ClinicId"] = new SelectList(_context.Clinics, "Id", "HouseNumb", doctor.ClinicId);
            ViewData["PlotId"] = new SelectList(_context.Plots, "Id", "Id", doctor.PlotId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Id", doctor.PositionId);
            ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Id", doctor.SpecialityId);
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include(d => d.Category)
                .Include(d => d.Clinic)
                .Include(d => d.Plot)
                .Include(d => d.Position)
                .Include(d => d.Speciality)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.Id == id);
        }
    }
}
