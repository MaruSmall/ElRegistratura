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


        //public async Task<IActionResult> CancelTicket(Guid id, [Bind("Id,Status,ScheduleId")] Ticket ticket)
        //{
        //    if (id != ticket.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            ticket.StatusId = 1;
        //            var s = _context.Tickets.Include(s => s.Schedule).Where(s => s.Id == id).AsNoTracking().FirstOrDefault();
        //            var t =
        //            (from tic in _context.Tickets
        //             where tic.Id == id
        //             select tic).AsNoTracking().FirstOrDefault();
        //            ticket.ScheduleId = s.ScheduleId;
        //            ticket.UserId = null;
        //            ticket.Time = t.Time;

        //            _context.Update(ticket);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!TicketExists(ticket.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    //ViewData["UserId"] = new SelectList(db.Users, "Id", "UserId", ticket.UserId);
        //    // ViewData["ScheduleId"] = new SelectList(db.Schedules, "Id", "Data", ticket.ScheduleId);
        //    //return View("Index");
        //    return Page();
        //}

        //private bool TicketExists(Guid id)
        //{
        //    return _context.Tickets.Any(e => e.Id == id);
        //}

        //[BindProperty]
        //public Ticket TicketCancel { get; set; }

        //public async Task<IActionResult> CancelTicket(Guid id)//отмена записи
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }
        //    TicketCancel.UserId = null;
        //    TicketCancel.StatusId = 1;
        //    _context.Attach(TicketCancel).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!TicketExists(TicketCancel.Id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return RedirectToPage("./Index");
        //}

        //private bool TicketExists(Guid id)
        //{
        //    return _context.Tickets.Any(e => e.Id == id);
        //}

    }
}
