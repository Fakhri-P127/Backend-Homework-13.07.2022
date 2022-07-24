using Backend_Homework_Pronia.DAL;
using Backend_Homework_Pronia.Models;
using Backend_Homework_Pronia.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
    //[Authorize(Roles ="Member")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(ApplicationDbContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Checkout()
        {
            Order order = new Order();
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

                //ViewBag.BasketItems = _context.BasketItems.Include(b=>b.Plant).Include(b=>b.AppUser).Where(b => b.AppUserId == user.Id).ToList();

                List<BasketItem> basketItems = _context.BasketItems
                    .Include(b => b.Plant).Include(b => b.AppUser)
                    .Where(b => b.AppUserId == user.Id).ToList();

                 order = new Order
                {
                     
                    BasketItems = basketItems,
                    AppUser = user,
                    TotalPrice = default
                };
                basketItems.ForEach(item => order.TotalPrice += item.Price * item.Quantity);

            }
            else
            {
                List<BasketItem> cookieBasketItems = new List<BasketItem>();

                string basketStr = HttpContext.Request.Cookies["Basket"];
                if (string.IsNullOrEmpty(basketStr))
                {
                    order.BasketItems = new List<BasketItem>();
                    return View(order);
                }
                else
                {
                    BasketVM basket = JsonConvert.DeserializeObject<BasketVM>(basketStr);
                    foreach (var item in basket.BasketCookieItemVMs)
                    {
                        Plant existed = _context.Plants.FirstOrDefault(p => p.Id == item.Id);
                        if (existed is null) return NotFound();

                        BasketItem basketItem = new BasketItem
                        {
                            Quantity = item.Quantity,
                            Plant = existed,
                            AppUser = null,
                            Price = existed.Price
                        };
                        cookieBasketItems.Add(basketItem);
                        //_context.BasketItems.Add(basketItem);
                    }
                    order = new Order
                    {
                        BasketItems = cookieBasketItems,
                        Date = DateTime.Now,
                        Status = null,
                        TotalPrice = default,
                        
                    };
                    cookieBasketItems.ForEach(item => order.TotalPrice += item.Price * item.Quantity);

                }
               

            }

            return View(order);

        }
        
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Checkout(Order order)
        {
            //if cookie basketStr null disa bu deyilse cookie ile yolunuda yaz.
            //baskete dbdaki mehsullari yazdir. LayoutService de GetBasket de if else yazmaq lazimdi.
            //eger cookie den gelirse indi olan yox db dan gelirse onuda yazmaq lazimdi :3 User isAuthenticated falan bele bir shey olmalidi mence
          
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                user.BasketItems = _context.BasketItems.Include(b => b.Plant).Include(b => b.AppUser).Where(b => b.AppUserId == user.Id).ToList();
                if (user.BasketItems.Count == 0)
                {
                    ModelState.AddModelError(string.Empty, "There's no items to buy");
                    return View(order);
                }
                order.BasketItems = user.BasketItems;
                order.AppUser = user;
                order.Date = DateTime.Now;
                order.Status = null;
                order.TotalPrice = default;
                user.BasketItems.ForEach(item => order.TotalPrice += item.Price * item.Quantity);
                if (!ModelState.IsValid)
                {
                    return View(order);
                }
                await _context.Orders.AddAsync(order);
                user.BasketItems = new List<BasketItem>();
                await _context.SaveChangesAsync();
            }
            else
            {
               // Isteyirdim ki guest uchun cookie de Id yaradim amma AspUserNet de foreign key di deye error verdi
               
                //string userIdReq = HttpContext.Request.Cookies["UserId"];
                //if (string.IsNullOrEmpty(userIdReq))
                //{
                //    HttpContext.Response.Cookies.Append("UserId", Guid.NewGuid().ToString());
                //    userIdReq = HttpContext.Request.Cookies["UserId"];
                //}
             
                string basketStr = HttpContext.Request.Cookies["Basket"];
                if (string.IsNullOrEmpty(basketStr))
                {
                    ModelState.AddModelError(string.Empty, "There's no items to buy");
                    order.BasketItems = new List<BasketItem>();
                    return View(order);
                }
              
                List<BasketItem> cookieBasketItems = new List<BasketItem>();
                BasketVM basket = JsonConvert.DeserializeObject<BasketVM>(basketStr);
                foreach (var item in basket.BasketCookieItemVMs)
                {
                    Plant existed = _context.Plants.FirstOrDefault(p => p.Id == item.Id);
                    if (existed is null) return NotFound();
                   
                    BasketItem basketItem = new BasketItem
                    {
                        Quantity = item.Quantity,
                        Plant = existed,
                        AppUserId = null,
                        Price = existed.Price
                    };
                    cookieBasketItems.Add(basketItem);
                }
                order.BasketItems = cookieBasketItems;
                order.AppUserId = null;
                order.Date = DateTime.Now;
                order.Status = null;
                order.TotalPrice = default;
                cookieBasketItems.ForEach(item => order.TotalPrice += item.Price * item.Quantity);
                if (!ModelState.IsValid)
                {
                    return View(order);
                }

                await _context.BasketItems.AddRangeAsync(cookieBasketItems);
                await _context.Orders.AddAsync(order);
                HttpContext.Response.Cookies.Delete("Basket");
                await _context.SaveChangesAsync();
            }
           

            
            return RedirectToAction("Index", "Home");
        }
    }
}
