using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelAgencyAPP.Models;
using TravelAgencyAPP.Models.Data;
using TravelAgencyAPP.ViewModels.Tours;

namespace TravelAgencyAPP.Controllers
{
    // [Authorize(Roles = "admin, registeredUser")]
    public class ToursController : Controller
    {
        private readonly AppCtx _context;
        private readonly UserManager<User> _userManager;

        public ToursController(AppCtx context, UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }

        // GET: Tours
        public async Task<IActionResult> Index()
        {

            //IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            var appCtx = _context.Tours
                
                /*.Include(s => s.Country)*/
                .Include(s => s.Hotel)
                .Include(s => s.User)
                .OrderBy(f => f.TourName);

            return View(await appCtx.ToListAsync());
        }

        // GET: Groups/Create
        public async Task<IActionResult> CreateAsync()
        {
            
           /* ViewData["IdCountry"] = new SelectList(_context.Countries, "Id", "CountryName");*/
            ViewData["IdHotel"] = new SelectList(_context.Hotels, "Id", "HotelName");
            ViewData["IdUser"] = new SelectList(_context.Users, "Id", "LastName");

            return View();
        }

        // POST: Storages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateToursViewModel model)
        {
            if (_context.Tours
                .Where(
                    f => f.IdHotel == model.IdHotel &&
                    f.TourName == model.TourName &&
                    f.IdUser == model.IdUser)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введенный тур уже существует");
            }

            if (ModelState.IsValid)
            {
            Tour tour = new()
            {
                TourName = model.TourName,
               /* IdCountry = model.IdCountry,*/
                IdHotel = model.IdHotel,
                NumberOfNights = model.NumberOfNights,
                IdUser = model.IdUser
            };

            _context.Add(tour);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
        // GET: Storages/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null || _context.Tours == null)
            {
                return NotFound();
            }

            var tour = await _context.Tours.FindAsync(id);
            if (tour == null)
            {
                return NotFound();
            }

            EditToursViewModel model = new()
            {
                Id = tour.Id,
                TourName = tour.TourName,
                IdHotel = tour.IdHotel,
                NumberOfNights = tour.NumberOfNights,
                IdUser = tour.IdUser
            };

            ViewData["IdHotel"] = new SelectList(_context.Hotels, "Id", "HotelName");
            ViewData["IdUser"] = new SelectList(_context.Users, "Id", "LastName");

            return View(model);
        }

        // POST: Storages/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, EditToursViewModel model)
        {
            Tour tour = await _context.Tours.FindAsync(id);

            if (_context.Tours
                .Where(
                    f => f.IdHotel == model.IdHotel &&
                    f.TourName == model.TourName &&
                    f.IdUser == model.IdUser)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введенный тур уже существует");
            }
            if (id != tour.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    tour.TourName = model.TourName;
                    tour.IdHotel = model.IdHotel;
                    tour.NumberOfNights = model.NumberOfNights;
                    tour.IdUser = model.IdUser;

                    _context.Update(tour);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TourExists(tour.Id))
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
            return View(model);
        }

        // GET: Storages/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null || _context.Tours == null)
            {
                return NotFound();
            }

            var tour = await _context.Tours
                .Include(s => s.Hotel)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tour == null)
            {
                return NotFound();
            }

            return View(tour);
        }

        // POST: Storages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            if (_context.Tours == null)
            {
                return Problem("Entity set 'AppCtx.Storages'  is null.");
            }
            var tour = await _context.Tours.FindAsync(id);
            if (tour != null)
            {
                _context.Tours.Remove(tour);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Storages/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null || _context.Tours == null)
            {
                return NotFound();
            }

            var tour = await _context.Tours
                .Include(s => s.Hotel)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tour == null)
            {
                return NotFound();
            }

            return View(tour);
        }

        private bool TourExists(short id)
        {
            return (_context.Tours?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}


