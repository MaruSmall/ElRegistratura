using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ElRegistratura.Data;
using ElRegistratura.Models;

namespace ElRegistratura.Areas.Identity.Pages.Account.Manage
{
    public class TicketsDetailsModel : PageModel
    {
        private readonly ElRegistratura.Data.ApplicationDbContext _context;

        public TicketsDetailsModel(ElRegistratura.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Ticket Ticket { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ticket = await _context.Tickets
                .Include(t => t.Schedule).ThenInclude(s=>s.Doctor).ThenInclude(s=>s.Clinic).ThenInclude(s=>s.Street)
                .Include(t=>t.Schedule).ThenInclude(t=>t.Cabinet)
                .Include(t => t.Status)
                
                .Include(t => t.User).FirstOrDefaultAsync(m => m.Id == id);

            if (Ticket == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
