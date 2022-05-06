using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElRegistratura.Data;
using ElRegistratura.Models;
using CSharpVitamins;

namespace ElRegistratura.Controllers
{
    public class SchedulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SchedulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Schedules
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Schedules.Include(s => s.Cabinet).Include(s => s.Doctor);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Schedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Cabinet)
                .Include(s => s.Doctor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // GET: Schedules/Create
        public IActionResult Create()
        {
            
           ViewData["CabinetId"] = new SelectList(_context.Cabinets, "Id", "Name");
           ViewData["DoctorFIO"] = new SelectList(_context.Doctors, "Id", "FIO");
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Data,DoctorId,WorkStart,WorkFinish,Duration,CabinetId,IsShow")] Schedule schedule, Ticket ticket, Status status)
        {
            TimeSpan hour = schedule.WorkFinish - schedule.WorkStart;
            int tick = Convert.ToInt32(hour.TotalMinutes / schedule.Duration.TotalMinutes); 

            if (ModelState.IsValid)
            {
                _context.Add(schedule);
                await _context.SaveChangesAsync();
                ticket.ScheduleId = schedule.Id;
                TimeSpan time = schedule.WorkStart;
                for (int i = 0; i < tick; i++)
                {
                    ticket.Id  = new Guid();
                    Random rnd = new Random();
                    
                    ticket.Number = new Random().Next(10000, 100000).ToString();
                    ticket.Time = time;
                    time = time + schedule.Duration;

                    ticket.StatusId = 1;
                    _context.Add(ticket);
                    await _context.SaveChangesAsync();
                }
               
                return RedirectToAction(nameof(Index));
            }
           
            ViewData["CabinetId"] = new SelectList(_context.Cabinets, "Id", "Name", schedule.CabinetId);
            ViewData["DoctorFIO"] = new SelectList(_context.Doctors, "Id", "FIO", schedule.DoctorId);
            return View(schedule);
        }

        // GET: Schedules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            ViewData["CabinetId"] = new SelectList(_context.Cabinets, "Id", "Name", schedule.CabinetId);
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "LastName", schedule.DoctorId);
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Data,DoctorId,WorkStart,WorkFinish,Duration,CabinetId,IsShow")] Schedule schedule)
        {
            if (id != schedule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(schedule.Id))
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
            ViewData["CabinetId"] = new SelectList(_context.Cabinets, "Id", "Name", schedule.CabinetId);
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "LastName", schedule.DoctorId);
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Cabinet)
                .Include(s => s.Doctor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(int id)
        {
            return _context.Schedules.Any(e => e.Id == id);
        }
    }
}
