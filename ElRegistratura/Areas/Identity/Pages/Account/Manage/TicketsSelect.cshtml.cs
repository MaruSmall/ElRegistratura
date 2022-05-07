using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ElRegistratura.Data;
using ElRegistratura.Models;
using System.Security.Claims;

namespace ElRegistratura.Areas.Identity.Pages.Account.Manage
{
    public class TicketsSelectModel : PageModel
    {
        private readonly ElRegistratura.Data.ApplicationDbContext _context;

        public TicketsSelectModel(ElRegistratura.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Ticket> Ticket { get;set; }

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Ticket = await _context.Tickets
                .Include(t => t.Schedule).ThenInclude(d=>d.Doctor).ThenInclude(c=>c.Clinic)
                .Include(t => t.Status)
                .Include(t => t.User).Where(p => p.UserId == userId).ToListAsync();
        }


    }
}
