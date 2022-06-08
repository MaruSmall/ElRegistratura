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
        public async Task<IActionResult> Index(int? doctor)
        {
            IQueryable<Schedule> schedule = _context.Schedules.Include(x => x.Cabinet).Include(d => d.Doctor);

            if (doctor != null && doctor != 0)
            {
                schedule = schedule.Where(x => x.DoctorId == doctor);
            }
            List<Doctor> statuList = _context.Doctors.ToList();
            statuList.Insert(0, new Doctor { FIOAndClinicName = "Все", Id = 0 });
            ViewModelScheduleIndex viewModelIndexTickets = new ViewModelScheduleIndex
            {
                Schedule = schedule,
                Doctor = new SelectList(statuList, "Id", "FIOAndClinicName", doctor)
            };
            return View(viewModelIndexTickets);

            //var applicationDbContext = _context.Schedules.Include(s => s.Cabinet).Include(s => s.Doctor);
            //return View(await applicationDbContext.ToListAsync());
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
            var doc = _context.Doctors.Where(s => s.Id == schedule.DoctorId).FirstOrDefault();
            var d=_context.Schedules.Where(s=>s.Data==schedule.Data ).FirstOrDefault();
            if(d!=null&& d.DoctorId == doc.Id)
            {
                   return RedirectToAction(nameof(Index));
   
            }
            else
            {
                var cabinet = _context.Cabinets.Where(s => s.Id == schedule.CabinetId).FirstOrDefault();
              
                
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
        public async Task<IActionResult> CreateWeek([Bind("Id,DateStart,Break, DateFinish,DoctorId,WorkStart,WorkFinish,Duration,CabinetId,IsShow")] Schedule schedule, Ticket ticket, Status status)
        {
            var cabinet = _context.Cabinets.Where(s => s.Id == schedule.CabinetId).FirstOrDefault();
            var doc = _context.Doctors.Where(s => s.Id == schedule.DoctorId).FirstOrDefault();
            var sc=_context.Schedules.Where(s=>s.WorkStart==schedule.WorkStart && s.WorkFinish==schedule.WorkFinish).FirstOrDefault();
           
                if (cabinet.ClinicId == doc.ClinicId)
                {
                    TimeSpan hour = schedule.WorkFinish - schedule.WorkStart;
                    int tick = Convert.ToInt32(hour.TotalMinutes / schedule.Duration.TotalMinutes);

                    if (ModelState.IsValid)
                    {
                        var countDays = schedule.DateFinish - schedule.DateStart;
                        var data = schedule.DateStart;

                        for (int k = 0; k <= countDays.Days; k++)
                        {
                      //  if(schedule.Data.DayOfWeek == DayOfWeek.Sunday)
                            schedule.Data = data;
                            var doct = _context.Doctors.Where(s => s.Id == schedule.DoctorId).FirstOrDefault();
                            var d = _context.Schedules.Where(s => s.Data == schedule.Data).ToList();
                        if (d.Count != 0)
                        {
                            for (int i = 0; i < d.Count; i++)
                            {
                                if (d[i].DoctorId == doct.Id)
                                {
                                    break;
                                }
                                else
                                {
                                    continue;
                                }

                            }
                            schedule.DateStart = schedule.DateStart.AddDays(1);
                            data = schedule.DateStart;
                            continue;
                        }
                        else
                        {
                                schedule.Id = new Guid();
                                schedule.DateStart = schedule.DateStart.AddDays(1);
                                data = schedule.DateStart;
                                _context.Add(schedule);
                                await _context.SaveChangesAsync();
                                ticket.ScheduleId = schedule.Id;
                                TimeSpan time = schedule.WorkStart;

                                for (int i = 0; i < tick; i++)
                                {
                                //if(schedule.Break==time)
                                //{
                                //    time = time + schedule.Duration;
                                //    continue;
                                //}
                                //else
                                //{

                                //if (schedule.CabinetId == cabinet.Id && schedule.Data == d[i].Data
                                //        && schedule.WorkStart == d[i].WorkStart && schedule.WorkFinish == d[i].WorkFinish)
                                //{
                                //    return RedirectToAction(nameof(Index));//проверка на занятость кабинете
                                //}
                                ticket.Id = new Guid();
                                    Random rnd = new Random();
                                    ticket.Number = new Random().Next(10000, 100000).ToString();
                                    ticket.Time = time;
                                    time = time + schedule.Duration;
                                    ticket.StatusId = 1;
                                    _context.Add(ticket);
                                    await _context.SaveChangesAsync();
                                //}
                                   
                                }
                            
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
        public IActionResult CreateSchedule()
        {
            CreateScheduleViewModel viewModel= new CreateScheduleViewModel();
           // viewModel.Schedule.Cabinet.CabinetNameAndClinicName = 
            ViewData["CabinetId"] = new SelectList(_context.Cabinets, "Id", "CabinetNameAndClinicName");
            ViewData["DoctorFIO"] = new SelectList(_context.Doctors, "Id", "FIOAndClinicName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSchedule([Bind("Id,DateStart,BreakStart,BreakFinish, DateFinish,DoctorId,WorkStart," +
            "WorkFinish,Duration,CabinetId,IsShow")] Schedule schedule, [Bind("ScheduleId")] Ticket ticket, Status status, bool Monday, bool Tuesday, 
            bool Wednesday, bool Thursday, bool Friday, bool Saturday, bool Sunday, 
            [Bind("WorkStartMonday,WorkFinishMonday,DurationMonday, BreakStartMonday,BreakFinishMonday, " +
            "WorkStartTuesday, WorkFinishTuesday,DurationTuesday,BreakStartTuesday,BreakFinishTuesday," +
            "WorkStartWednesday, WorkFinishWednesday,DurationWednesday,BreakStartWednesday,BreakFinishWednesday,"+
            "WorkStartThursday, WorkFinishThursday,DurationThursday,BreakStartThursday,BreakFinishThursday,"+
            "WorkStartFriday, WorkFinishFriday,DurationFriday,BreakStartFriday,BreakFinishFriday,"+
            "WorkStartSaturday, WorkFinishSaturday,DurationSaturday,BreakStartSaturday,BreakFinishSaturday,"+
            "WorkStartSunday, WorkFinishSunday,DurationSunday,BreakStartSunday,BreakFinishSunday")] CreateScheduleViewModel vm, 
            [Bind("Id,DateStart,BreakStart,BreakFinish, DateFinish,DoctorId,WorkStart," +
            "WorkFinish,Duration,CabinetId,IsShow")] Schedule schedule1, [Bind("ScheduleId")] Ticket ticket1, 
            [Bind("Id,DateStart,BreakStart,BreakFinish, DateFinish,DoctorId,WorkStart," +
            "WorkFinish,Duration,CabinetId,IsShow")] Schedule schedule2, [Bind("ScheduleId")] Ticket ticket2, 
            [Bind("Id,DateStart,BreakStart,BreakFinish, DateFinish,DoctorId,WorkStart," +
            "WorkFinish,Duration,CabinetId,IsShow")] Schedule schedule3, [Bind("ScheduleId")] Ticket ticket3,
            [Bind("Id,DateStart,BreakStart,BreakFinish, DateFinish,DoctorId,WorkStart," +
            "WorkFinish,Duration,CabinetId,IsShow")] Schedule schedule4, [Bind("ScheduleId")] Ticket ticket4,
            [Bind("Id,DateStart,BreakStart,BreakFinish, DateFinish,DoctorId,WorkStart," +
            "WorkFinish,Duration,CabinetId,IsShow")] Schedule schedule5, [Bind("ScheduleId")] Ticket ticket5,
            [Bind("Id,DateStart,BreakStart,BreakFinish, DateFinish,DoctorId,WorkStart," +
            "WorkFinish,Duration,CabinetId,IsShow")] Schedule schedule6, [Bind("ScheduleId")] Ticket ticket6)
        {
            var cabinet = _context.Cabinets.Where(s => s.Id == schedule.CabinetId).FirstOrDefault();
            var doc = _context.Doctors.Where(s => s.Id == schedule.DoctorId).FirstOrDefault();
         
            if (ModelState.IsValid)
            {
                if (cabinet.ClinicId == doc.ClinicId)
                {
                    var countDays = schedule.DateFinish - schedule.DateStart;
                    var data = schedule.DateStart;
                    for (int i = 0; i <= countDays.Days; i++)
                    {
                        if (Monday == true && data.DayOfWeek == DayOfWeek.Monday)
                        {
                            i++;
                            bool z = false;
                            schedule.Data = data;
                            var doct = _context.Doctors.Where(s => s.Id == schedule.DoctorId).FirstOrDefault();
                            var d = _context.Schedules.Where(s => s.Data == schedule.Data).ToList();
                            if (d.Count != 0)
                            {
                                for (int j = 0; j < d.Count; j++)
                                {
                                    if (schedule.CabinetId == d[j].CabinetId && schedule.Data == d[j].Data
                                        && vm.WorkStartMonday == d[j].WorkStart && vm.WorkFinishMonday == d[j].WorkFinish)
                                    {
                                        schedule.DateStart = schedule.DateStart.AddDays(1);
                                        data = schedule.DateStart;
                                        z = true;
                                       
                                        break;
                                    }
                                    else
                                    {
                                        if (d[j].DoctorId == doct.Id)
                                        {
                                            schedule.DateStart = schedule.DateStart.AddDays(1);
                                            data = schedule.DateStart;
                                            z = true;
                                            
                                            break;
                                        }
                                        else
                                        {
                                            continue;
                                        }

                                    }

                                }
                                
                            }
                            if(d.Count==0)
                            {
                                var sc = _context.Schedules.Where(s => s.Data == schedule.Data).FirstOrDefault();
                                if (sc!=null&&schedule.CabinetId == sc.CabinetId && schedule.Data == sc.Data
                                        && vm.WorkStartMonday == sc.WorkStart && vm.WorkFinishMonday == sc.WorkFinish)
                                {
                                    z = true;
                                    schedule.DateStart = schedule.DateStart.AddDays(1);
                                    data = schedule.DateStart;
                                    break;
                                }
                                else
                                {
                                    z = true;
                                    if(ticket==null)
                                    {
                                        ticket = new Ticket();
                                        CreateScheduleAndTicketsMonday(schedule, ticket, data, vm);
                                        //schedule.DateStart = schedule.DateStart.AddDays(1);
                                        data = schedule.DateStart;
                                        ticket = null;
                                    }
                                    else
                                    {
                                        CreateScheduleAndTicketsMonday(schedule, ticket, data, vm);
                                        //schedule.DateStart = schedule.DateStart.AddDays(1);
                                        data = schedule.DateStart;
                                        ticket = null;
                                    }
                                }
                            }
                            if(z==false)
                            {
                                if (ticket == null)
                                {
                                    ticket = new Ticket();
                                    CreateScheduleAndTicketsMonday(schedule, ticket, data, vm);
                                    //schedule.DateStart = schedule.DateStart.AddDays(1);
                                    data = schedule.DateStart;
                                    ticket = null;
                                }
                                else
                                {
                                    
                                    CreateScheduleAndTicketsMonday(schedule, ticket, data, vm);
                                    //schedule.DateStart = schedule.DateStart.AddDays(1);
                                    data = schedule.DateStart;
                                    ticket = null;
                                }
                            }
                        }
                        if (Tuesday == true && data.DayOfWeek == DayOfWeek.Tuesday)
                        {
                            i++;
                            schedule1.Data = data;
                            schedule1.CabinetId = schedule.CabinetId;
                            schedule1.DoctorId= schedule.DoctorId;
                            schedule1.IsShow = schedule.IsShow;
                            var doct = _context.Doctors.Where(s => s.Id == schedule.DoctorId).FirstOrDefault();
                            var d = _context.Schedules.Where(s => s.Data == schedule1.Data).ToList();
                            bool z = false;
                            if (d.Count != 0)
                            {
                                for (int j = 0; j < d.Count; j++)
                                {
                                    if (schedule.CabinetId == d[j].CabinetId && schedule1.Data == d[j].Data
                                        && vm.WorkStartTuesday == d[j].WorkStart && vm.WorkFinishTuesday == d[j].WorkFinish)
                                    {
                                        schedule.DateStart = schedule.DateStart.AddDays(1);
                                        data = schedule.DateStart;
                                        z=true;
                                       
                                        break;
                                    }
                                    else
                                    {
                                        if (d[j].DoctorId == doct.Id)
                                        {
                                            schedule.DateStart = schedule.DateStart.AddDays(1);
                                            data = schedule.DateStart;
                                            z =true;
                                           
                                            break;
                                        }
                                        else
                                        {
                                            continue;
                                        }

                                    }

                                }
                            }
                            if(d.Count==0)
                            {
                                var sc = _context.Schedules.Where(s => s.Data == schedule1.Data).FirstOrDefault();
                                if (sc != null && schedule.CabinetId == sc.CabinetId && schedule1.Data == sc.Data
                                        && vm.WorkStartTuesday == sc.WorkStart && vm.WorkFinishTuesday == sc.WorkFinish)
                                {
                                    z = true;
                                    schedule.DateStart = schedule.DateStart.AddDays(1);
                                    data = schedule.DateStart;
                                    break;
                                    
                                }
                                else
                                {
                                    z = true;
                                    if(ticket1==null)
                                    {
                                        ticket1 = new Ticket();
                                        CreateScheduleAndTicketsTuesday(schedule1, ticket1, data, vm);
                                        schedule.DateStart = schedule.DateStart.AddDays(1);
                                        data = schedule.DateStart;
                                        ticket1 = null;
                                    }
                                    else
                                    {
                                        CreateScheduleAndTicketsTuesday(schedule1, ticket1, data, vm);
                                        schedule.DateStart = schedule.DateStart.AddDays(1);
                                        data = schedule.DateStart;
                                        ticket1 = null;
                                    }
                                }
                            }
                            if(z==false)
                            {
                                if (ticket1 == null)
                                {
                                    ticket1 = new Ticket();
                                    CreateScheduleAndTicketsTuesday(schedule1, ticket1, data, vm);
                                    schedule.DateStart = schedule.DateStart.AddDays(1);
                                    data = schedule.DateStart;
                                    ticket1 = null;
                                }
                                else
                                {
                                    CreateScheduleAndTicketsTuesday(schedule1, ticket1, data, vm);
                                    schedule.DateStart = schedule.DateStart.AddDays(1);
                                    data = schedule.DateStart;
                                    ticket1 = null;
                                }

                            }
                        }

                        if (Wednesday == true && data.DayOfWeek == DayOfWeek.Wednesday)
                        {
                            i++;
                            schedule2.Data = data;
                            schedule2.CabinetId = schedule.CabinetId;
                            schedule2.DoctorId = schedule.DoctorId;
                            schedule2.IsShow = schedule.IsShow;
                            var doct = _context.Doctors.Where(s => s.Id == schedule.DoctorId).FirstOrDefault();
                            var d = _context.Schedules.Where(s => s.Data == schedule2.Data).ToList();
                            bool z = false;
                            if (d.Count != 0)
                            {
                                for (int j = 0; j < d.Count; j++)
                                {
                                    if (schedule.CabinetId == d[j].CabinetId && schedule2.Data == d[j].Data
                                        && vm.WorkStartWednesday == d[j].WorkStart && vm.WorkFinishWednesday == d[j].WorkFinish)
                                    {
                                        schedule.DateStart = schedule.DateStart.AddDays(1);
                                        data = schedule.DateStart;
                                        z = true;
                                      
                                        break;
                                    }
                                    else
                                    {
                                        if (d[j].DoctorId == doct.Id)
                                        {
                                            schedule.DateStart = schedule.DateStart.AddDays(1);
                                            data = schedule.DateStart;
                                            z=true;
                                            
                                            break;
                                        }
                                        else
                                        {
                                           
                                            continue;
                                           
                                        }

                                    }
                                }
                            }
                            if (d.Count == 0)
                            {
                                var sc = _context.Schedules.Where(s => s.Data == schedule2.Data).FirstOrDefault();
                                if (sc != null && schedule.CabinetId == sc.CabinetId && schedule2.Data == sc.Data
                                        && vm.WorkStartWednesday == sc.WorkStart && vm.WorkFinishWednesday == sc.WorkFinish)
                                {
                                    z = true;
                                    schedule.DateStart = schedule.DateStart.AddDays(1);
                                    data = schedule.DateStart;
                                    break;
                                }
                                else
                                {
                                    z = true;
                                    if (ticket2 == null)
                                    {
                                        ticket2 = new Ticket();
                                        CreateScheduleAndTicketsWednesday(schedule2, ticket2, data, vm);
                                        schedule.DateStart = schedule.DateStart.AddDays(1);
                                        data = schedule.DateStart;
                                        ticket2 = null;
                                    }
                                    else
                                    {
                                        CreateScheduleAndTicketsWednesday(schedule2, ticket2, data, vm);
                                        schedule.DateStart = schedule.DateStart.AddDays(1);
                                        data = schedule.DateStart;
                                        ticket2 = null;
                                    }
                                }
                            }
                            if (z == false)
                            {
                                if (ticket2 == null)
                                {
                                    ticket2 = new Ticket();
                                    CreateScheduleAndTicketsWednesday(schedule2, ticket2, data, vm);
                                    schedule.DateStart = schedule.DateStart.AddDays(1);
                                    data = schedule.DateStart;
                                    ticket2 = null;
                                }
                                else
                                {
                                    CreateScheduleAndTicketsWednesday(schedule2, ticket2, data, vm);
                                    schedule.DateStart = schedule.DateStart.AddDays(1);
                                    data = schedule.DateStart;
                                    ticket2 = null;
                                }
                            }
                        }
                        if (Thursday == true && data.DayOfWeek == DayOfWeek.Thursday)
                        {
                            i++;
                            schedule3.Data = data;
                            schedule3.CabinetId = schedule.CabinetId;
                            schedule3.DoctorId = schedule.DoctorId;
                            schedule3.IsShow = schedule.IsShow;
                            var doct = _context.Doctors.Where(s => s.Id == schedule.DoctorId).FirstOrDefault();
                            var d = _context.Schedules.Where(s => s.Data == schedule3.Data).ToList();
                            bool z = false;
                            if (d.Count != 0)
                            {
                                for (int j = 0; j < d.Count; j++)
                                {
                                    if (schedule.CabinetId == d[j].CabinetId && schedule3.Data == d[j].Data
                                        && vm.WorkStartThursday == d[j].WorkStart && vm.WorkFinishThursday == d[j].WorkFinish)
                                    {
                                        schedule.DateStart = schedule.DateStart.AddDays(1);
                                        data = schedule.DateStart;
                                        z = true;
                                       
                                        break;
                                    }
                                    else
                                    {
                                        if (d[j].DoctorId == doct.Id)
                                        {
                                            schedule.DateStart = schedule.DateStart.AddDays(1);
                                            data = schedule.DateStart;
                                            z = true;
                                           
                                            break;
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                }
                            }
                            if (d.Count == 0)
                            {
                                var sc = _context.Schedules.Where(s => s.Data == schedule3.Data).FirstOrDefault();
                                if (sc != null && schedule.CabinetId == sc.CabinetId && schedule3.Data == sc.Data
                                        && vm.WorkStartThursday == sc.WorkStart && vm.WorkFinishThursday == sc.WorkFinish)
                                {
                                    z = true;
                                    schedule.DateStart = schedule.DateStart.AddDays(1);
                                    data = schedule.DateStart;
                                    break;
                                }
                                else
                                {
                                    z = true;
                                    if (ticket3 == null)
                                    {
                                        ticket3 = new Ticket();
                                        CreateScheduleAndTicketsThursday(schedule3, ticket3, data, vm);
                                        schedule.DateStart = schedule.DateStart.AddDays(1);
                                        data = schedule.DateStart;
                                        ticket3 = null;
                                    }
                                    else
                                    {
                                        CreateScheduleAndTicketsThursday(schedule3, ticket3, data, vm);
                                        schedule.DateStart = schedule.DateStart.AddDays(1);
                                        data = schedule.DateStart;
                                        ticket3 = null;
                                    }

                                }
                            }
                            if(z==false)
                            {
                                if (ticket3 == null)
                                {
                                    ticket3 = new Ticket();
                                    CreateScheduleAndTicketsThursday(schedule3, ticket3, data, vm);
                                    schedule.DateStart = schedule.DateStart.AddDays(1);
                                    data = schedule.DateStart;
                                    ticket3 = null;
                                }
                                else
                                {
                                    CreateScheduleAndTicketsThursday(schedule3, ticket3, data, vm);
                                    schedule.DateStart = schedule.DateStart.AddDays(1);
                                    data = schedule.DateStart;
                                    ticket3 = null;
                                }

                            }
                        }

                        if (Friday == true && data.DayOfWeek == DayOfWeek.Friday)
                        {
                            i++;
                            schedule4.Data = data;
                            schedule4.CabinetId = schedule.CabinetId;
                            schedule4.DoctorId = schedule.DoctorId;
                            schedule4.IsShow = schedule.IsShow;
                            var doct = _context.Doctors.Where(s => s.Id == schedule.DoctorId).FirstOrDefault();
                            var d = _context.Schedules.Where(s => s.Data == schedule4.Data).ToList();
                            bool z = false;
                            if (d.Count != 0)
                            {
                                for (int j = 0; j < d.Count; j++)
                                {
                                    if (schedule.CabinetId == d[j].CabinetId && schedule4.Data == d[j].Data
                                        && vm.WorkStartFriday == d[j].WorkStart && vm.WorkFinishFriday == d[j].WorkFinish)
                                    {
                                        schedule.DateStart = schedule.DateStart.AddDays(1);
                                        data = schedule.DateStart;
                                        z = true;
                                        break;
                                    }
                                    else
                                    {
                                        if (d[j].DoctorId == doct.Id)
                                        {
                                            schedule.DateStart = schedule.DateStart.AddDays(1);
                                            data = schedule.DateStart;
                                            z = true;
                                            break;
                                        }
                                        else
                                        {
                                            continue;
                                            
                                        }

                                    }

                                }
                            }
                            if (d.Count == 0)
                            {
                                var sc = _context.Schedules.Where(s => s.Data == schedule4.Data).FirstOrDefault();
                                if (sc != null && schedule.CabinetId == sc.CabinetId && schedule4.Data == sc.Data
                                        && vm.WorkStartFriday == sc.WorkStart && vm.WorkFinishFriday == sc.WorkFinish)
                                {
                                    z = true;
                                    schedule.DateStart = schedule.DateStart.AddDays(1);
                                    data = schedule.DateStart;
                                    break;
                                }
                                else
                                {
                                    z = true;
                                    if (ticket4 == null)
                                    {
                                        ticket4 = new Ticket();
                                        CreateScheduleAndTicketsFriday(schedule4, ticket4, data, vm);
                                        schedule.DateStart = schedule.DateStart.AddDays(1);
                                        data = schedule.DateStart;
                                        ticket4 = null;
                                    }
                                    else
                                    {
                                        CreateScheduleAndTicketsFriday(schedule4, ticket4, data, vm);
                                        schedule.DateStart = schedule.DateStart.AddDays(1);
                                        data = schedule.DateStart;
                                        ticket4 = null;
                                    }

                                }
                            }
                            if(z==false)
                            {
                                if (ticket4 == null)
                                {
                                    ticket4 = new Ticket();
                                    CreateScheduleAndTicketsFriday(schedule4, ticket4, data, vm);
                                    schedule.DateStart = schedule.DateStart.AddDays(1);
                                    data = schedule.DateStart;
                                    ticket4 = null;
                                }
                                else
                                {
                                    CreateScheduleAndTicketsFriday(schedule4, ticket4, data, vm);
                                    schedule.DateStart = schedule.DateStart.AddDays(1);
                                    data = schedule.DateStart;
                                    ticket4 = null;
                                }
                            }
                        }

                        if (Saturday == true && data.DayOfWeek == DayOfWeek.Saturday)
                        {
                            i++;
                            schedule5.Data = data;
                            schedule5.CabinetId = schedule.CabinetId;
                            schedule5.DoctorId = schedule.DoctorId;
                            schedule5.IsShow = schedule.IsShow;
                            var doct = _context.Doctors.Where(s => s.Id == schedule.DoctorId).FirstOrDefault();
                            var d = _context.Schedules.Where(s => s.Data == schedule5.Data).ToList();
                            bool z=false;
                            if (d.Count != 0)
                            {
                                for (int j = 0; j < d.Count; j++)
                                {
                                    if (schedule.CabinetId == d[j].CabinetId && schedule5.Data == d[j].Data
                                        && vm.WorkStartSaturday == d[j].WorkStart && vm.WorkFinishSaturday == d[j].WorkFinish)
                                    {
                                        schedule.DateStart = schedule.DateStart.AddDays(1);
                                        data = schedule.DateStart;
                                        z = true;
                                       
                                        break;
                                    }
                                    else
                                    {
                                        if (d[j].DoctorId == doct.Id)
                                        {
                                            schedule.DateStart = schedule.DateStart.AddDays(1);
                                            data = schedule.DateStart;
                                            z = true;
                                            
                                            break;
                                        }
                                        else
                                        {
                                            continue;
                                        }

                                    }

                                }
                               

                            }
                            if (d.Count == 0)
                            {
                                var sc = _context.Schedules.Where(s => s.Data == schedule5.Data).FirstOrDefault();
                                if (sc != null && schedule.CabinetId == sc.CabinetId && schedule5.Data == sc.Data
                                        && vm.WorkStartSaturday == sc.WorkStart && vm.WorkFinishSaturday == sc.WorkFinish)
                                {
                                    z = true;
                                    schedule.DateStart = schedule.DateStart.AddDays(1);
                                    data = schedule.DateStart;
                                    break;

                                }
                                else
                                {
                                    z = true;
                                    if (ticket5 == null)
                                    {
                                        ticket5 = new Ticket();
                                        CreateScheduleAndTicketsSaturday(schedule5, ticket5, data, vm);
                                        schedule.DateStart = schedule.DateStart.AddDays(1);
                                        data = schedule.DateStart;
                                        ticket5 = null;
                                    }
                                    else
                                    {
                                        CreateScheduleAndTicketsSaturday(schedule5, ticket5, data, vm);
                                        schedule.DateStart = schedule.DateStart.AddDays(1);
                                        data = schedule.DateStart;
                                        ticket5 = null;
                                    }

                                }
                            }
                            if(z==false)
                            {
                                if (ticket5 == null)
                                {
                                    ticket5 = new Ticket();
                                    CreateScheduleAndTicketsSaturday(schedule5, ticket5, data, vm);
                                    schedule.DateStart = schedule.DateStart.AddDays(1);
                                    data = schedule.DateStart;
                                    ticket5 = null;
                                }
                                else
                                {
                                    CreateScheduleAndTicketsSaturday(schedule5, ticket5, data, vm);
                                    schedule.DateStart = schedule.DateStart.AddDays(1);
                                    data = schedule.DateStart;
                                    ticket5 = null;
                                }
                            }
                        }

                        if (Sunday == true && data.DayOfWeek == DayOfWeek.Sunday)
                        {
                            i++;
                            schedule6.Data = data;
                            schedule6.CabinetId = schedule.CabinetId;
                            schedule6.DoctorId = schedule.DoctorId;
                            schedule6.IsShow = schedule.IsShow;
                            var doct = _context.Doctors.Where(s => s.Id == schedule.DoctorId).FirstOrDefault();
                            var d = _context.Schedules.Where(s => s.Data == schedule6.Data).ToList();
                            bool z = false;
                            if (d.Count != 0)
                            {
                                for (int j = 0; j < d.Count; j++)
                                {
                                    if (schedule.CabinetId == d[j].CabinetId && schedule6.Data == d[j].Data
                                        && vm.WorkStartSunday == d[j].WorkStart && vm.WorkFinishSunday == d[j].WorkFinish)
                                    {
                                        schedule.DateStart = schedule.DateStart.AddDays(1);
                                        data = schedule.DateStart;
                                        z = true;
                                       
                                        break;
                                    }
                                    else
                                    {
                                        if (d[j].DoctorId == doct.Id)
                                        {
                                            schedule.DateStart = schedule.DateStart.AddDays(1);
                                            data = schedule.DateStart;
                                            z=true;
                                           
                                            break;
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                }
                            }
                            if (d.Count == 0)
                            {
                                var sc = _context.Schedules.Where(s => s.Data == schedule6.Data).FirstOrDefault();
                                if (sc != null && schedule.CabinetId == sc.CabinetId && schedule6.Data == sc.Data
                                        && vm.WorkStartSunday == sc.WorkStart && vm.WorkFinishSunday == sc.WorkFinish)
                                {
                                    z= true;
                                    schedule.DateStart = schedule.DateStart.AddDays(1);
                                    data = schedule.DateStart;
                                    break;

                                }
                                else
                                {
                                    z = true;
                                    if (ticket6 == null)
                                    {
                                        ticket6 = new Ticket();
                                        CreateScheduleAndTicketsSunday(schedule6, ticket6, data, vm);
                                        schedule.DateStart = schedule.DateStart.AddDays(1);
                                        data = schedule.DateStart;
                                        ticket6 = null;
                                    }
                                    else
                                    {
                                        CreateScheduleAndTicketsSunday(schedule6, ticket6, data, vm);
                                        schedule.DateStart = schedule.DateStart.AddDays(1);
                                        data = schedule.DateStart;
                                        ticket6 = null;
                                    }
                                }
                            }
                            if(z==false)
                            {
                                if (ticket6 == null)
                                {
                                    ticket6 = new Ticket();
                                    CreateScheduleAndTicketsSunday(schedule6, ticket6, data, vm);
                                    schedule.DateStart = schedule.DateStart.AddDays(1);
                                    data = schedule.DateStart;
                                    ticket6 = null;
                                }
                                else
                                {
                                    CreateScheduleAndTicketsSunday(schedule6, ticket6, data, vm);
                                    schedule.DateStart = schedule.DateStart.AddDays(1);
                                    data = schedule.DateStart;
                                    ticket6 = null;
                                }
                            }
                        }
                        if(data.DayOfWeek == DayOfWeek.Monday)
                        {
                            continue;
                        }
                        else
                        {
                            if (data == schedule.DateStart)
                            {
                                data = data.AddDays(1);
                                schedule.DateStart = schedule.DateStart.AddDays(1);
                            }
                            else
                            {
                                data = schedule.DateStart;
                            }
                        }
                        
                    }
                }
            }

            ViewData["CabinetId"] = new SelectList(_context.Cabinets, "Id", "CabinetNameAndClinicName");
            ViewData["DoctorFIO"] = new SelectList(_context.Doctors, "Id", "FIOAndClinicName");
            
            return View();

        }

        private void CreateScheduleAndTicketsMonday(Schedule schedule, [Bind("ScheduleId")] Ticket ticket, DateTime data, CreateScheduleViewModel vm )
        {
            
            TimeSpan hour = (TimeSpan)(vm.WorkFinishMonday - vm.WorkStartMonday);
            TimeSpan dura = (TimeSpan)(vm.DurationMonday);
            int tick = Convert.ToInt32(hour.TotalMinutes / dura.TotalMinutes);
            schedule.Id = new Guid();
            schedule.DateStart = schedule.DateStart.AddDays(1);
            data = schedule.DateStart;
            schedule.WorkStart = (TimeSpan)vm.WorkStartMonday;
            schedule.WorkFinish = (TimeSpan)vm.WorkFinishMonday;
            schedule.Duration = (TimeSpan)vm.DurationMonday;
            _context.Add(schedule);
            _context.SaveChanges();
           
            TimeSpan time = (TimeSpan)vm.WorkStartMonday;

            for (int k = 0; k <= tick; k++)
            {
                if (vm.BreakStartMonday <= time && vm.BreakFinishMonday > time)
                {
                    time = (TimeSpan)vm.BreakFinishMonday;
                    continue;
                }
                else
                {
                    ticket.Id = new Guid();
                    ticket.ScheduleId = schedule.Id;
                    ticket.Number = new Random().Next(10000, 100000).ToString();
                    ticket.Time = time;
                    time = time + (TimeSpan)vm.DurationMonday;
                    ticket.StatusId = 1;
                    _context.Add(ticket);
                    _context.SaveChanges();
                }
            }
            schedule.Tickets.Clear();
            _context.SaveChanges();
            return;
        }
        private void CreateScheduleAndTicketsTuesday(Schedule schedule, [Bind("ScheduleId")] Ticket ticket, DateTime data, CreateScheduleViewModel vm)
        {
            ticket = new Ticket();
            TimeSpan hour = (TimeSpan)(vm.WorkFinishTuesday - vm.WorkStartTuesday);
            TimeSpan dura = (TimeSpan)(vm.DurationTuesday);
            int tick = Convert.ToInt32(hour.TotalMinutes / dura.TotalMinutes);
            schedule.Id = new Guid();
            schedule.DateStart = schedule.DateStart.AddDays(1);
            data = schedule.DateStart;
            schedule.WorkStart = (TimeSpan)vm.WorkStartTuesday;
            schedule.WorkFinish = (TimeSpan)vm.WorkFinishTuesday;
            schedule.Duration = (TimeSpan)vm.DurationTuesday;
            _context.Add(schedule);
            _context.SaveChanges();
            ticket.ScheduleId = schedule.Id;
            TimeSpan time = (TimeSpan)vm.WorkStartTuesday;

            for (int k = 0; k <= tick; k++)
            {
                if (vm.BreakStartTuesday <= time && vm.BreakFinishTuesday > time)
                {
                    time = (TimeSpan)vm.BreakFinishTuesday;
                    continue;
                }
                else
                {
                    ticket.Id = new Guid();
                    ticket.Number = new Random().Next(10000, 100000).ToString();
                    ticket.Time = time;
                    time = time + (TimeSpan)vm.DurationTuesday;
                    ticket.StatusId = 1;
                    _context.Add(ticket);
                    _context.SaveChanges();
                }

            }
            schedule.Tickets.Clear();
            _context.SaveChanges();
            return;
        }


        private void CreateScheduleAndTicketsWednesday(Schedule schedule, Ticket ticket, DateTime data, CreateScheduleViewModel vm)
        {
            TimeSpan hour = (TimeSpan)(vm.WorkFinishWednesday - vm.WorkStartWednesday);
            TimeSpan dura = (TimeSpan)(vm.DurationWednesday);
            int tick = Convert.ToInt32(hour.TotalMinutes / dura.TotalMinutes);
            schedule.Id = new Guid();
            schedule.DateStart = schedule.DateStart.AddDays(1);
            data = schedule.DateStart;
            schedule.WorkStart = (TimeSpan)vm.WorkStartWednesday;
            schedule.WorkFinish = (TimeSpan)vm.WorkFinishWednesday;
            schedule.Duration = (TimeSpan)vm.DurationWednesday;
            _context.Add(schedule);
            _context.SaveChanges();
            ticket.ScheduleId = schedule.Id;
            TimeSpan time = (TimeSpan)vm.WorkStartWednesday;

            for (int k = 0; k <= tick; k++)
            {
                if (vm.BreakStartWednesday <= time && vm.BreakFinishWednesday > time)
                {
                    time = (TimeSpan)vm.BreakFinishWednesday;
                    continue;
                }
                else
                {
                    ticket.Id = new Guid();
                    ticket.Number = new Random().Next(10000, 100000).ToString();
                    ticket.Time = time;
                    time = time + (TimeSpan)vm.DurationWednesday;
                    ticket.StatusId = 1;
                    _context.Add(ticket);
                    _context.SaveChanges();
                }

            }
            schedule.Tickets.Clear();
            _context.SaveChanges();
            return;
        }

        private void CreateScheduleAndTicketsThursday(Schedule schedule, Ticket ticket, DateTime data, CreateScheduleViewModel vm)
        {
            TimeSpan hour = (TimeSpan)(vm.WorkFinishThursday - vm.WorkStartThursday);
            TimeSpan dura = (TimeSpan)(vm.DurationThursday);
            int tick = Convert.ToInt32(hour.TotalMinutes / dura.TotalMinutes);
            schedule.Id = new Guid();
            schedule.DateStart = schedule.DateStart.AddDays(1);
            data = schedule.DateStart;
            schedule.WorkStart = (TimeSpan)vm.WorkStartThursday;
            schedule.WorkFinish = (TimeSpan)vm.WorkFinishThursday;
            schedule.Duration = (TimeSpan)vm.DurationThursday;
            _context.Add(schedule);
            _context.SaveChanges();
            ticket.ScheduleId = schedule.Id;
            TimeSpan time = (TimeSpan)vm.WorkStartThursday;

            for (int k = 0; k <= tick; k++)
            {
                if (vm.BreakStartThursday <= time && vm.BreakFinishThursday > time)
                {
                    time = (TimeSpan)vm.BreakFinishThursday;
                    continue;
                }
                else
                {
                    ticket.Id = new Guid();
                    ticket.Number = new Random().Next(10000, 100000).ToString();
                    ticket.Time = time;
                    time = time + (TimeSpan)vm.DurationThursday;
                    ticket.StatusId = 1;
                    _context.Add(ticket);
                    _context.SaveChanges();
                }
                
            }
            schedule.Tickets.Clear();
            _context.SaveChanges();
            return;
        }

        private void CreateScheduleAndTicketsFriday(Schedule schedule, Ticket ticket, DateTime data, CreateScheduleViewModel vm)
        {
            TimeSpan hour = (TimeSpan)(vm.WorkFinishFriday - vm.WorkStartFriday);
            TimeSpan dura = (TimeSpan)(vm.DurationFriday);
            int tick = Convert.ToInt32(hour.TotalMinutes / dura.TotalMinutes);
            schedule.Id = new Guid();
            schedule.DateStart = schedule.DateStart.AddDays(1);
            data = schedule.DateStart;
            schedule.WorkStart = (TimeSpan)vm.WorkStartFriday;
            schedule.WorkFinish = (TimeSpan)vm.WorkFinishFriday;
            schedule.Duration = (TimeSpan)vm.DurationFriday;
            _context.Add(schedule);
            _context.SaveChanges();
            ticket.ScheduleId = schedule.Id;
            TimeSpan time = (TimeSpan)vm.WorkStartFriday;

            for (int k = 0; k <= tick; k++)
            {
                if (vm.BreakStartFriday <= time && vm.BreakFinishFriday > time)
                {
                    time = (TimeSpan)vm.BreakFinishFriday;
                    continue;
                }
                else
                {
                    ticket.Id = new Guid();
                    ticket.Number = new Random().Next(10000, 100000).ToString();
                    ticket.Time = time;
                    time = time + (TimeSpan)vm.DurationFriday;
                    ticket.StatusId = 1;
                    _context.Add(ticket);
                    _context.SaveChanges();
                }
            }
            schedule.Tickets.Clear();
            _context.SaveChanges();
            return;
        }

        private void CreateScheduleAndTicketsSaturday(Schedule schedule, Ticket ticket, DateTime data, CreateScheduleViewModel vm)
        {
            TimeSpan hour = (TimeSpan)(vm.WorkFinishSaturday - vm.WorkStartSaturday);
            TimeSpan dura = (TimeSpan)(vm.DurationSaturday);
            int tick = Convert.ToInt32(hour.TotalMinutes / dura.TotalMinutes);
            schedule.Id = new Guid();
            schedule.DateStart = schedule.DateStart.AddDays(1);
            data = schedule.DateStart;
            schedule.WorkStart = (TimeSpan)vm.WorkStartSaturday;
            schedule.WorkFinish = (TimeSpan)vm.WorkFinishSaturday;
            schedule.Duration = (TimeSpan)vm.DurationSaturday;
            _context.Add(schedule);
            _context.SaveChanges();
            ticket.ScheduleId = schedule.Id;
            TimeSpan time = (TimeSpan)vm.WorkStartSaturday;

            for (int k = 0; k <= tick; k++)
            {
                if (vm.BreakStartSaturday <= time && vm.BreakFinishSaturday > time)
                {
                    time = (TimeSpan)vm.BreakFinishSaturday;
                    continue;
                }
                else
                {
                    ticket.Id = new Guid();
                    ticket.Number = new Random().Next(10000, 100000).ToString();
                    ticket.Time = time;
                    time = time + (TimeSpan)vm.DurationSaturday;
                    ticket.StatusId = 1;
                    _context.Add(ticket);
                    _context.SaveChanges();
                }

            }
            schedule.Tickets.Clear();
            _context.SaveChanges();
            return;
        }

        private void CreateScheduleAndTicketsSunday(Schedule schedule, Ticket ticket, DateTime data, CreateScheduleViewModel vm)
        {
            TimeSpan hour = (TimeSpan)(vm.WorkFinishSunday - vm.WorkStartSunday);
            TimeSpan dura = (TimeSpan)(vm.DurationSunday);
            int tick = Convert.ToInt32(hour.TotalMinutes / dura.TotalMinutes);
            schedule.Id = new Guid();
            schedule.DateStart = schedule.DateStart.AddDays(1);
            data = schedule.DateStart;
            schedule.WorkStart = (TimeSpan)vm.WorkStartSunday;
            schedule.WorkFinish = (TimeSpan)vm.WorkFinishSunday;
            schedule.Duration = (TimeSpan)vm.DurationSunday;
            _context.Add(schedule);
            _context.SaveChanges();
            ticket.ScheduleId = schedule.Id;
            TimeSpan time = (TimeSpan)vm.WorkStartSunday;

            for (int k = 0; k <= tick; k++)
            {
                if (vm.BreakStartSunday <= time && vm.BreakFinishSunday > time)
                {
                    time = (TimeSpan)vm.BreakFinishSunday;
                    continue;
                }
                else
                {
                    ticket.Id = new Guid();
                    ticket.Number = new Random().Next(10000, 100000).ToString();
                    ticket.Time = time;
                    time = time + (TimeSpan)vm.DurationSunday;
                    ticket.StatusId = 1;
                    _context.Add(ticket);
                    _context.SaveChanges();
                }

            }
            schedule.Tickets.Clear();
            _context.SaveChanges();
            return;
        }
        private bool ScheduleExists(Guid id)
        {
            return _context.Schedules.Any(e => e.Id == id);
        }
    }
}
