using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelAgencyAPP.Models;
using TravelAgencyAPP.Models.Data;
using TravelAgencyAPP.ViewModels.Countries;

namespace TravelAgencyAPP.Controllers
{
    public class CountriesController : Controller
    {
        private readonly AppCtx _context;

        public CountriesController(AppCtx context)
        {
            _context = context;
        }

        // GET: Countries
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "country_desc" : "";
            ViewData["CurrentFilter"] = searchString;

            var countries = from c in _context.Countries
                           select c;
            if (!String.IsNullOrEmpty(searchString))
            {
                countries = countries.Where(s => s.CountryName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "country_desc":
                    countries = countries.OrderByDescending(c => c.CountryName);
                    break;
                default:
                    countries = countries.OrderBy(c => c.CountryName);
                    break;
            }
            return View(await countries.AsNoTracking().ToListAsync());
        }

        // GET: Countries/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // GET: Countries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Countries/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCountryViewModel model)
        {
            if (_context.Countries
                .Where(f => f.CountryName == model.CountryName)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеная страна уже существует");
            }

            if (ModelState.IsValid)
            {
                Country country = new()
                {
                    CountryName = model.CountryName
                };

                _context.Add(country);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            EditCountryViewModel model = new()
            {
                Id = country.Id,
                CountryName = country.CountryName
            };
            return View(model);
        }

        // POST: Countries/Edit/5
   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, EditCountryViewModel model)
        {
            Country country = await _context.Countries.FindAsync(id);

            if (_context.Countries
               .Where(f => f.CountryName == model.CountryName)
               .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеная страна уже существует");
            }

            if (id != country.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    country.CountryName = model.CountryName;
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.Id))
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

        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            if (_context.Countries == null)
            {
                return Problem("Entity set 'AppCtx.Countries'  is null.");
            }
            var country = await _context.Countries.FindAsync(id);
            if (country != null)
            {
                _context.Countries.Remove(country);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CountryExists(short id)
        {
          return (_context.Countries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
