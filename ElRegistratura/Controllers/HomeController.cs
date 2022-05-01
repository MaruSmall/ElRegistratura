using ElRegistratura.Data;
using ElRegistratura.Data.Data;
using ElRegistratura.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ElRegistratura.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;
        public Guid DItem;
        private ApplicationDbContext db;
        public HomeController(ApplicationDbContext context, UserManager<User> userManager)
        {
            db = context;
            _userManager = userManager;
           
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult AdminView()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ClinicsView()//поликлиники
        {
            return View(db.Clinics.Include(s => s.Street).ToList());
        }
        [HttpGet]
        public IActionResult SpecialityDoctorsView(int? id)//специальность определенной поликлиники
        {
            var s = (from spec in db.Specialities
                     from doc in db.Doctors
                     where spec.Id == doc.SpecialityId && doc.ClinicId == id
                     select spec).ToList().Distinct();
            return View(s);
        }

        [HttpGet]
        public IActionResult DoctorsView(int id)//врачи определенной специальности в определенной поликлиники
        {
           
            var doctors = (from doc in db.Doctors.Include(s => s.Speciality)
                           where doc.SpecialityId == id
                           select doc).ToList().Distinct();
            if (doctors == null)
            {
                return NotFound();
            }

            return View(doctors);
        }
        [HttpGet]
        public IActionResult ScheduleDoctorsView(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var schedules = (from sch in db.Schedules.Include(d => d.Doctor)
                             where sch.DoctorId == id
                             select sch).ToList();
            if (schedules == null)
            {
                return NotFound();
            }
            DoctorItem.IdDoctorItem = id;
            return View(schedules);
        }
        [HttpGet]
        public IActionResult TicketsView(int id)//талоны
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedules = (from sch in db.Tickets.Include(d => d.Schedule)
                             where sch.ScheduleId == id
                             select sch).ToList();
            if (schedules == null)
            {
                return NotFound();
            }
            //DoctorItem.IdTicket= id;
            return View(schedules);
        }
        [Authorize]
        public IActionResult CheckDataView(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
           
            var viewModel = new ViewModelTicket();
            viewModel.Ticket = db.Tickets

                .Include(s => s.Schedule).ThenInclude(s => s.Doctor)
                .ThenInclude(s => s.Clinic).ThenInclude(s => s.Street)
                .Include(s => s.Schedule).ThenInclude(s => s.Doctor).ThenInclude(s => s.Speciality)
                .Where(s => s.Id == id).AsNoTracking().ToList();

            return View(viewModel);
        }
        // [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTicket(Guid id, [Bind("Id,Status,ScheduleId")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    // var sche= db.Schedules.Include(s => s.Tickets).Where(s => s.Tickets.Id==ticket.Id);

                    ticket.StatusId = 2;

                    var s = db.Tickets.Include(s => s.Schedule).Where(s => s.Id == id).AsNoTracking().FirstOrDefault();
                    //var t = db.Tickets.Include(s => s.Time).Where(s => s.Id == id).AsNoTracking().FirstOrDefault();
                    var t =
                    (from tic in db.Tickets
                     where tic.Id == id
                     select tic).AsNoTracking().FirstOrDefault();
                    ticket.ScheduleId = s.ScheduleId;
                    ticket.UserId = userId;
                    ticket.Time = t.Time;

                    db.Update(ticket);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            //ViewData["UserId"] = new SelectList(db.Users, "Id", "UserId", ticket.UserId);
            // ViewData["ScheduleId"] = new SelectList(db.Schedules, "Id", "Data", ticket.ScheduleId);
            return View("Index");
        }

        public async Task<IActionResult> CancelTicket(Guid id, [Bind("Id,Status,ScheduleId")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ticket.StatusId = 1;
                    var s = db.Tickets.Include(s => s.Schedule).Where(s => s.Id == id).AsNoTracking().FirstOrDefault();
                    var t =
                    (from tic in db.Tickets
                     where tic.Id == id
                     select tic).AsNoTracking().FirstOrDefault();
                    ticket.ScheduleId = s.ScheduleId;
                    ticket.UserId = null;
                    ticket.Time = t.Time;

                    db.Update(ticket);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            //ViewData["UserId"] = new SelectList(db.Users, "Id", "UserId", ticket.UserId);
            // ViewData["ScheduleId"] = new SelectList(db.Schedules, "Id", "Data", ticket.ScheduleId);
            //return View("Index");
            return View("./TicketsSelect");
        }


        private bool TicketExists(Guid id)
        {
            return db.Tickets.Any(e => e.Id == id);
        }

        public void ScheduleIdTicket(int id, [Bind("Id,Time,Status,PatientId,ScheduleId")] Ticket ticket)
        {


        }

        [HttpGet]
        public IActionResult SearchDoctor(string searchString)
        {

           
            var doctors = from m in db.Doctors
                          select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                doctors = doctors.Where(s => s.LastName.Contains(searchString));
            }

          
            return View( doctors.ToList());



        }
        [HttpGet]
        public IActionResult SearchTicket()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
