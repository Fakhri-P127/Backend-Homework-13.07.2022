using Backend_Homework_Pronia.DAL;
using Backend_Homework_Pronia.Models;
using Backend_Homework_Pronia.Service;
using Backend_Homework_Pronia.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_Homework_Pronia.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleInManager;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _http;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleInManager,ApplicationDbContext context, IHttpContextAccessor http)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleInManager = roleInManager;
            _context = context;
            _http = http;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (!ModelState.IsValid) return View();
            if (!register.Terms)
            {
                ModelState.AddModelError("", "Please check our terms");
                return View();

            }
            AppUser user = new AppUser()
            {
                Firstname = register.Firstname,
                Lastname = register.Lastname,
                UserName=register.Username,
                Email=register.Email, 
            };

            IdentityResult result = await _userManager.CreateAsync(user, register.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(user, "Member");
            return RedirectToAction("Index","Home");
        }
       
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = await _userManager.FindByNameAsync(login.Username);
            if (user is null) return View();
            user.BasketItems = await _context.BasketItems
                .Include(b => b.Plant).Include(b => b.AppUser)
                .Where(b => b.AppUserId == user.Id).ToListAsync();

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user,login.Password,login.RememberMe,true);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "You made too many attempts. Wait for a while");
                    return View();
                }
                ModelState.AddModelError("", "Username or password incorrect.");
                return View();
            }

            string basketStr = HttpContext.Request.Cookies["Basket"];
            BasketVM basket;
            if (!string.IsNullOrEmpty(basketStr))
            {
                basket = JsonConvert.DeserializeObject<BasketVM>(basketStr);
                foreach (var item in basket.BasketCookieItemVMs)
                {
                    // Eger eyni mehsuldan userBasket de varsa onda quantitysini artir, birde mehsulu yeniden yaratma.
                    if (user.BasketItems.Any(b => b.PlantId == item.Id))
                    {
                        user.BasketItems.FirstOrDefault(b => b.PlantId == item.Id).Quantity += item.Quantity;
                        continue;
                    }
                    Plant existed = _context.Plants.FirstOrDefault(p => p.Id == item.Id);
                    if (existed is null) return NotFound();

                    BasketItem basketItem = new BasketItem
                    {
                        Quantity = item.Quantity,
                        Plant = existed,
                        AppUser = user,
                        Price = existed.Price
                    };
                    _context.BasketItems.Add(basketItem);

                    // layout service den instance almaqimin sebebi odur ki, normalda login olanda cookie deki datalardan basketItem yaradiriq(Database) amma bunlar basket e dushmur. Bunun uchun getBasket() metodunu cagiririq. 
                    LayoutService layoutService = new LayoutService(_context, _http, _userManager);
                    await layoutService.GetBasket();
                }
                HttpContext.Response.Cookies.Delete("Basket");
                _context.SaveChanges();
            }
            
            return RedirectToAction("Index", "Home");

        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public JsonResult ShowAuthentication()
        {
            return Json(User.Identity.IsAuthenticated);
        }

        public async Task CreateRoles()
        {
            await _roleInManager.CreateAsync(new IdentityRole("Member"));
            await _roleInManager.CreateAsync(new IdentityRole("Moderator"));
            await _roleInManager.CreateAsync(new IdentityRole("Admin"));
        }
    }
}
