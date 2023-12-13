using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using TravelAgencyAPP.Models;
using TravelAgencyAPP.Models.Data;
using TravelAgencyAPP.ViewModels.Hotels;

namespace TravelAgencyAPP.Controllers
{
    /*    [Authorize(Roles = "admin, registeredUser")]*/
    public class HotelsController : Controller
    {
        private readonly AppCtx _context;
        private readonly UserManager<User> _userManager;

        public HotelsController(AppCtx context, UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }

        // GET: Specialties
        public async Task<IActionResult> Index()
        {
            // IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            var appCtx = _context.Hotels
                .Include(s => s.Country)                    // связываем специальности с формами обучения
                .OrderBy(f => f.HotelName);                          // сортировка по коду специальности
            return View(await appCtx.ToListAsync());            // полученный результат передаем в представление списком
        }

        // GET: Specialties/Create
        public async Task<IActionResult> CreateAsync()
        {
            // IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            // при отображении страницы заполняем элемент "выпадающий список" формами обучения
            // при этом указываем, что в качестве идентификатора используется поле "Id"
            // а отображать пользователю нужно поле "FormOfEdu" - название формы обучения
            ViewData["IdCountry"] = new SelectList(_context.Countries, "Id", "CountryName");
            return View();
        }

        // POST: Specialties/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateHotelViewModel model)
        {
            // IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (_context.Hotels
                .Where(f => f.IdCountry == model.IdCountry &&
                    f.HotelName == model.HotelName &&
                    f.Review == model.Review)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Такое название отеля уже существует");
            }

            if (ModelState.IsValid)
            {
                // если введены корректные данные,
                // то создается экземпляр класса модели Specialty, т.е. формируется запись в таблицу Specialties
                Hotel hotel = new()
                {
                    HotelName = model.HotelName,
                    NumberOfStars = model.NumberOfStars,
                    Rating = model.Rating,
                    Review = model.Review,
                    // с помощью свойства модели получим идентификатор выбранной формы обучения пользователем
                    IdCountry = model.IdCountry
                };

                _context.Add(hotel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdCountry"] = new SelectList(
                _context.Countries,
                "Id", "CountryName", model.IdCountry);
            return View(model);
        }

        // GET: Specialties/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            EditHotelViewModel model = new()
            {
                Id = hotel.Id,
                HotelName = hotel.HotelName,
                NumberOfStars = hotel.NumberOfStars,
                Rating = hotel.Rating,
                Review = hotel.Review,
                IdCountry = hotel.IdCountry
            };

            // IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            // в списке в качестве текущего элемента устанавливаем значение из базы данных,
            // указываем параметр specialty.IdFormOfStudy
            ViewData["IdCountry"] = new SelectList(
                _context.Countries,
                "Id", "CountryName", hotel.IdCountry);
            return View(model);
        }

        // POST: Specialties/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, EditHotelViewModel model)
        {
            Hotel hotel = await _context.Hotels.FindAsync(id);

            if (_context.Hotels
                .Where(f => f.IdCountry == model.IdCountry &&
                    f.HotelName == model.HotelName)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Вы не изменили название отеля");
            }
            if (id != hotel.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    hotel.HotelName = model.HotelName;
                    hotel.NumberOfStars = model.NumberOfStars;
                    hotel.Rating = model.Rating;
                    hotel.Review = model.Review;
                    hotel.IdCountry = model.IdCountry;
                    _context.Update(hotel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelExists(hotel.Id))
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
            // IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            ViewData["IdCountry"] = new SelectList(
                _context.Countries,
                "Id", "CountryName", hotel.IdCountry);
            return View(model);
        }

        // GET: Specialties/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels
                .Include(s => s.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // POST: Specialties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Specialties/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels
                .Include(s => s.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        private bool HotelExists(short id)
        {
            return _context.Hotels.Any(e => e.Id == id);
        }
    }
}