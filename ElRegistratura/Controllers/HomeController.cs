using ElRegistratura.Data;
using ElRegistratura.Data.Data;
using ElRegistratura.Email;
using ElRegistratura.Models;
using GemBox.Document;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;

namespace ElRegistratura.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db;
        private readonly IWebHostEnvironment environment;
        private readonly IEmailSender _emailSender;
        public static ViewModelTicket viewModel = new ViewModelTicket();

        readonly IDataProtector _protector;


        public HomeController(ApplicationDbContext context, IEmailSender emailSender,
            IWebHostEnvironment environment, IDataProtectionProvider provider)
        {
            db = context;

            this.environment = environment;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            _emailSender = emailSender;

            _protector = provider.CreateProtector("DataProtectionDemo.Controllers.HomeController");

        }

        [Route("")]
        [Route("Home")]
        [Route("Home/Index")]
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
        [HttpGet]
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
        public IActionResult SpecialityDoctorsView(int id)//специальность определенной поликлиники
        {

            //var url1 = HttpUtility.UrlDecode(id);
            //url1 = url1.Remove(0, url1.Length - 2);
            //url1 = url1.Trim(new char[] { '/' });
         
            if (id==null)
            {
                return NotFound();
            }
          //  int idint = Convert.ToInt32(url1);
            
            DoctorItem.IdDoctorItem = id;
            var s = (from spec in db.Specialities
                     from doc in db.Doctors
                     where spec.Id == doc.SpecialityId && doc.ClinicId == id
                     select spec).ToList().Distinct();

            return View(s);
        }

        [HttpGet]
        public IActionResult DoctorsView(int id)//врачи определенной специальности в определенной поликлиники
        {


            var doc = db.Doctors.Include(s => s.Speciality).Include(c => c.Clinic)
                .Where(s => s.SpecialityId == id && s.ClinicId == DoctorItem.IdDoctorItem).ToList().Distinct();
            return View(doc);
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
            //DoctorItem.IdDoctorItem = id;
            return View(schedules);
        }

        [HttpGet]
        public IActionResult TicketsView(Guid id)//талоны
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
        [HttpGet]
        [Authorize]
        public IActionResult CheckDataView(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var idTicket= db.Tickets.Include(s=>s.Schedule).Where(s=>s.Id == id).First();
            var idCabinet = db.Schedules.Include(s=>s.Cabinet).Where(s=>s.Id==idTicket.ScheduleId).FirstOrDefault();
            ViewModelTicket.Ticket = db.Tickets
                .Include(s => s.Schedule).ThenInclude(s => s.Doctor)
                .ThenInclude(s => s.Clinic).ThenInclude(s => s.Street)
                .Include(s => s.Schedule).ThenInclude(s => s.Doctor)
                .ThenInclude(s => s.Clinic).ThenInclude(s=>s.Cabinets.Where(s=>s.Id==idCabinet.CabinetId))
                .Include(s => s.Schedule).ThenInclude(s => s.Doctor).ThenInclude(s => s.Speciality)
                .Where(s => s.Id == id).AsNoTracking().ToList();
            var user = db.Users.Where(s => s.Id == userId).FirstOrDefault();
            var sche = db.Schedules.Include(s => s.Cabinet).Where(s => s.Id == idTicket.ScheduleId).First();
            var doc = db.Doctors.Include(s => s.Speciality).Include(s => s.Clinic).ThenInclude(s=>s.Street)
                .Where(s => s.Id == sche.DoctorId).First();
            ViewModelTicket.fioDoctor = doc.FIO;
            ViewModelTicket.fioUser = userId;
            ViewModelTicket.fioUser = user.LastName + " " + user.FirstName + " " + user.Patronymic;
            ViewModelTicket.spec = doc.Speciality.Name;
            ViewModelTicket.address = doc.Clinic.Street.Name + " д." + doc.Clinic.HouseNumb + " к." + doc.Clinic.Housing;
            ViewModelTicket.onClinic = doc.Clinic.Name;
            ViewModelTicket.cabinet = sche.Cabinet.Name;
            ViewModelTicket.date = sche.Data.ToShortDateString();
            ViewModelTicket.time = Convert.ToString(idTicket.Time);
            ViewModelTicket.time = ViewModelTicket.time.Remove(ViewModelTicket.time.Length - 3);
            ViewModelTicket.number = idTicket.Number;
            return View(viewModel);
           
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTicketPost(Guid id, [Bind("Id,StatusId,UserId")] Ticket ticket, IFormFile file)//84d9cf17-3224-40cc-9f1e-08da2e5d1d29
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

                    ticket.StatusId = 2;
                    var s = db.Tickets.Include(s => s.Schedule).Where(s => s.Id == id).AsNoTracking().FirstOrDefault();

                    var t =
                            (from tic in db.Tickets
                             where tic.Id == id
                             select tic).AsNoTracking().FirstOrDefault();
                    var n = db.Tickets.Where(s => s.Id == id).AsNoTracking().FirstOrDefault();
                    ticket.ScheduleId = s.ScheduleId;
                    ticket.UserId = userId;
                    ticket.Time = t.Time;
                    ticket.Number = n.Number;
                    db.Update(ticket);
                    await db.SaveChangesAsync();
                    var user = db.Users.Include(s => s.Street).Where(s => s.Id == userId).AsNoTracking().FirstOrDefault();

                    var path = Path.Combine(this.environment.ContentRootPath, "InvoiceWithFields.docx");
                    var document = DocumentModel.Load(path);

                    // Execute mail merge process.
                    document.MailMerge.Execute(viewModel);

                    document.Save("Талон" + " " + ViewModelTicket.fioUser + ".pdf");

                    string pathDoc = Path.Combine(this.environment.ContentRootPath, "Талон" + " " + ViewModelTicket.fioUser + ".pdf");
                    using (var stream = System.IO.File.OpenRead(pathDoc))
                    {
                        var files = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name));
                        
                        var message = new Message(new string[] { user.Email }, "Талон на прием",
                        "Талон на прием в прикрипленом файле", files);
                        await _emailSender.SendEmailAsync(message);
                    }

                    FileInfo fileInf = new FileInfo(pathDoc);
                    if (fileInf.Exists)
                    {
                        fileInf.Delete();
                    }
                    return RedirectToAction("MessageTicketGood");

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

            }

            return View("Index");
        }

        public async Task<IActionResult> CancelTicket(Guid id, [Bind("Id,StatusId,UserId")] Ticket ticket)
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
                    var n = db.Tickets.Where(s => s.Id == id).AsNoTracking().FirstOrDefault();
                    ticket.ScheduleId = s.ScheduleId;
                    ticket.UserId = null;
                    ticket.Time = t.Time;
                    ticket.Number = n.Number;

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
               
                return View("MessageTicketCancel");
            }
            
            return View("./Index");
        }

        private bool TicketExists(Guid id)
        {
            return db.Tickets.Any(e => e.Id == id);
        }

        [HttpGet]
        public IActionResult SearchDoctor(string searchString)
        {
            var doctors = from m in db.Doctors
                          select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                string[] words = searchString.Split(' ');
                foreach (string word in words)
                {
                    doctors = doctors.Where(s => s.LastName.Contains(word) || s.FirstName.Contains(word) || s.Patronymic.Contains(word));
                }
               
            }

            return View( doctors.ToList());
        }

        [HttpGet]
        public IActionResult SearchTicket(string searchString)//c66bd62c-25c7-48dd-a6a2-e895ba414f27
        {

            var ticket = db.Tickets.Where(s => s.Number == " ");
            if(!String.IsNullOrEmpty(searchString))//1871108196
            {
                var tickets = db.Tickets.Include(s => s.Status).Where(s => s.Number == searchString);
                return View(tickets.ToList());
            }
            return View(ticket);
        }

        public IActionResult MessageTicketGood()
        {
           
            return View();
        }
        public IActionResult MessageTicketCancel()
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
