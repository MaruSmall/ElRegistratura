using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElRegistratura.Models;

namespace ElRegistratura.Data
{
    public class PrimerModel : PageModel
    {
        private readonly ElRegistratura.Data.ApplicationDbContext _context;

        public PrimerModel(ElRegistratura.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Doctor Doctor { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Doctor = await _context.Doctors
                .Include(d => d.Category)
                .Include(d => d.Clinic)
                .Include(d => d.Plot)
                .Include(d => d.Position)
                .Include(d => d.Speciality).FirstOrDefaultAsync(m => m.Id == id);

            if (Doctor == null)
            {
                return NotFound();
            }
           ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
           ViewData["ClinicId"] = new SelectList(_context.Clinics, "Id", "HouseNumb");
           ViewData["PlotId"] = new SelectList(_context.Plots, "Id", "Id");
           ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Id");
           ViewData["SpecialityId"] = new SelectList(_context.Specialities, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Doctor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(Doctor.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.Id == id);
        }
    }
}
