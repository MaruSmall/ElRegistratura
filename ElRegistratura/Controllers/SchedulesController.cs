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
        public async Task<IActionResult> Details(Guid? id)
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
            
           ViewData["CabinetId"] = new SelectList(_context.Cabinets, "Id", "CabinetNameAndClinicName");
           ViewData["DoctorFIO"] = new SelectList(_context.Doctors, "Id", "FIOAndClinicName");
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Data,DoctorId,WorkStart,WorkFinish,Duration,CabinetId,IsShow")] Schedule schedule, Ticket ticket, Status status)
        {
            var d=_context.Schedules.Where(s=>s.Data==schedule.Data).FirstOrDefault();
            if(d!=null)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var cabinet = _context.Cabinets.Where(s => s.Id == schedule.CabinetId).FirstOrDefault();
                var doc=_context.Doctors.Where(s=>s.Id== schedule.DoctorId).FirstOrDefault();
                if(cabinet.ClinicId==doc.ClinicId)
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
                            ticket.Id = new Guid();
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

                    ViewData["CabinetId"] = new SelectList(_context.Cabinets, "Id", "CabinetNameAndClinicName", schedule.CabinetId);
                    ViewData["DoctorFIO"] = new SelectList(_context.Doctors, "Id", "FIOAndClinicName", schedule.DoctorId);
                    return View(schedule);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
               
            }
           
           
        }

        // GET: Schedules/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
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
            ViewData["CabinetId"] = new SelectList(_context.Cabinets, "Id", "CabinetNameAndClinicName", schedule.CabinetId);
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "FIOAndClinicName", schedule.DoctorId);
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Data,DoctorId,WorkStart,WorkFinish,Duration,CabinetId,IsShow")] Schedule schedule)
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
            ViewData["CabinetId"] = new SelectList(_context.Cabinets, "Id", "CabinetNameAndClinicName", schedule.CabinetId);
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "FIOAndClinicName", schedule.DoctorId);
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
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
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        public IActionResult CreateWeek()
        {

            ViewData["CabinetId"] = new SelectList(_context.Cabinets, "Id", "CabinetNameAndClinicName");
            ViewData["DoctorFIO"] = new SelectList(_context.Doctors, "Id", "FIOAndClinicName");
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWeek([Bind("Id,DateStart, DateFinish,DoctorId,WorkStart,WorkFinish,Duration,CabinetId,IsShow")] Schedule schedule, Ticket ticket, Status status)
        {
            //var d = _context.Schedules.Where(s => s.Data == schedule.Data).ToList();
            //if (d != null)
            //{
            //    return RedirectToAction(nameof(Index));
            //}
            //else
            //{

            var cabinet = _context.Cabinets.Where(s => s.Id == schedule.CabinetId).FirstOrDefault();
            var doc = _context.Doctors.Where(s => s.Id == schedule.DoctorId).FirstOrDefault();
            if (cabinet.ClinicId == doc.ClinicId)
            {

                TimeSpan hour = schedule.WorkFinish - schedule.WorkStart;
                int tick = Convert.ToInt32(hour.TotalMinutes / schedule.Duration.TotalMinutes);

                if (ModelState.IsValid)
                {
                    var countDays = schedule.DateFinish - schedule.DateStart;
                    var data = schedule.DateStart;
                    //schedule.Data = schedule.DateStart;

                    for (int k = 0; k <= countDays.Days; k++)
                    {
                    var dp = _context.Schedules.Select(s=>s.Data).ToList();
                    foreach(var d in dp)
                    {
                        if(d==data) 
                        {
                            continue;
                        }
                    }
                       schedule.Id = new Guid();
                       schedule.Data = data;
                       schedule.DateStart = schedule.DateStart.AddDays(1);
                       data = schedule.DateStart;
                       _context.Add(schedule);
                       await _context.SaveChangesAsync();

                       ticket.ScheduleId = schedule.Id;
                       TimeSpan time = schedule.WorkStart;

                       for (int i = 0; i < tick; i++)
                       {
                           ticket.Id = new Guid();
                           Random rnd = new Random();

                           ticket.Number = new Random().Next(10000, 100000).ToString();
                           ticket.Time = time;
                           time = time + schedule.Duration;

                           ticket.StatusId = 1;
                           _context.Add(ticket);
                           await _context.SaveChangesAsync();
                       }
                            
                        
                       
                    }


                    return RedirectToAction(nameof(Index));
                }

                ViewData["CabinetId"] = new SelectList(_context.Cabinets, "Id", "CabinetNameAndClinicName", schedule.CabinetId);
                ViewData["DoctorFIO"] = new SelectList(_context.Doctors, "Id", "FIOAndClinicName", schedule.DoctorId);
                return View(schedule);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
           
        }

        private bool ScheduleExists(Guid id)
        {
            return _context.Schedules.Any(e => e.Id == id);
        }
    }
}
