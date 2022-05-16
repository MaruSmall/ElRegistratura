using ElRegistratura.Data;
using ElRegistratura.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ElRegistratura.Controllers
{
    public class UsersController : Controller
    {

        private readonly ApplicationDbContext _context;
        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserController
        public async Task<IActionResult> Index()
        {

            var applicationDbContext = _context.Users.Include(c => c.Street).Include(s=>s.Sex);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: UserController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(c => c.Street).Include(s=>s.Sex)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);

        }

        // GET: UserController/Create
        public IActionResult Create()
        {
            ViewData["StreetId"] = new SelectList(_context.Street, "Id", "Name");
            ViewData["SexId"] = new SelectList(_context.Sex, "Id", "Name");
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LastName,FirstName,Patronymic,StreetId,HouseNumber,Housing,PhoneNumber,SexId")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["SexId"] = new SelectList(_context.Sex, "Id", "Name", user.SexId);
            ViewData["StreetId"] = new SelectList(_context.Street, "Id", "Name", user.StreetId);
            return View(user);
        }

        // GET: UserController/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["StreetId"] = new SelectList(_context.Street, "Id", "Name", user.StreetId);
            ViewData["SexId"] = new SelectList(_context.Sex, "Id", "Name", user.SexId);
            return View(user);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,LastName,FirstName,Patronymic,StreetId,HouseNumber,Housing,PhoneNumber,SexId")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            ViewData["StreetId"] = new SelectList(_context.Street, "Id", "Name", user.StreetId);
            ViewData["SexId"] = new SelectList(_context.Sex, "Id", "Name", user.SexId);
            return View(user);
        }

        // GET: UserController/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(c => c.Street).Include(s=>s.Sex)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
