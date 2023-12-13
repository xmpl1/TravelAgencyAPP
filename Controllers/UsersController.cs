using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAgencyAPP.Models.Data;
using TravelAgencyAPP.ViewModels.Users;

namespace TravelAgencyAPP.Controllers
{
    public class UsersController : Controller
    {
        UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        // отображение списка пользователей
        // действия для начальной страницы Index
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateRegSortParm"] = sortOrder == "Date" ? "datereg_desc" : "Date";
            ViewData["DateWorkingSortParm"] = sortOrder == "Date1" ? "dateworking_desc" : "Date1";
            ViewData["CurrentFilter"] = searchString;

            var users = from u in _userManager.Users
                        select u;
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(u => u.LastName);
                    break;
                case "Date":
                    users = users.OrderBy(u => u.DateReg);
                    break;
                case "datereg_desc":
                    users = users.OrderByDescending(u => u.DateReg);
                    break;
                case "Date1":
                    users = users.OrderBy(u => u.DateWorking);
                    break;
                case "dateworking_desc":
                    users = users.OrderByDescending(u => u.DateWorking);
                    break;
                default:
                    users = users.OrderBy(u => u.LastName);
                    break;
            }
            return View(await users.AsNoTracking().ToListAsync());
        }


        // действия для создания пользователя Create
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Email = model.Email,
                    UserName = model.Email,
                    LastName = model.LastName,
                    FirstName = model.FirstName,
                    DateReg = DateTime.Now,
                    DateWorking = model.DateWorking
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }


        // действия для изменения пользователя Edit
        public async Task<IActionResult> Edit(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                LastName = user.LastName,
                FirstName = user.FirstName,
                DateReg = DateTime.Now,
                DateWorking = user.DateWorking,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.LastName = model.LastName;
                    user.FirstName = model.FirstName;
                    user.DateReg = model.DateReg;
                    user.DateWorking = model.DateWorking;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }


        // действия для удаления пользователя Delete с подтверждением
        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            IdentityResult result = await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }
    }
}