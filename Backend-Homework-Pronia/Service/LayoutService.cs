using Backend_Homework_Pronia.DAL;
using Backend_Homework_Pronia.Models;
using Backend_Homework_Pronia.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_Homework_Pronia.Service
{
    public class LayoutService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _http;
        private readonly UserManager<AppUser> _userManager;

        public LayoutService(ApplicationDbContext context, IHttpContextAccessor http,UserManager<AppUser> userManager)
        {
            _context = context;
            _http = http;
            _userManager = userManager;
        }
        public List<Setting> GetSettings()
        {
            List<Setting> settings = _context.Settings.ToList();
            return settings;
        }

        public async Task<LayoutBasketVM> GetBasket()
        {
            LayoutBasketVM layoutBasket = new LayoutBasketVM();
            layoutBasket.BasketItemVMs = new List<BasketItemVM>();

            if (_http.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser user = await  _userManager.FindByNameAsync(_http.HttpContext.User.Identity.Name);
                user.BasketItems = await _context.BasketItems
                    .Include(b => b.AppUser).Include(b => b.Plant)
                    .Where(b => b.AppUserId == user.Id).ToListAsync();

                layoutBasket.TotalPrice = 0;

                foreach (var basketItem in user.BasketItems.ToList())
                {
                    Plant existed = await _context.Plants.Include(p => p.PlantImages).FirstOrDefaultAsync(p => p.Id == basketItem.PlantId);
                    if(existed is null)
                    {
                        user.BasketItems.Remove(basketItem);
                        continue;
                    }
                    BasketItemVM basketItemVM = new BasketItemVM
                    {
                        Plant = existed,
                        Quantity = basketItem.Quantity
                    };
                    layoutBasket.BasketItemVMs.Add(basketItemVM);
                    layoutBasket.TotalPrice += existed.Price * basketItem.Quantity;
                }
            }
            else
            {
                string basketStr = _http.HttpContext.Request.Cookies["Basket"];

                if (string.IsNullOrEmpty(basketStr))
                {
                    return layoutBasket;
                }
                BasketVM basket = JsonConvert.DeserializeObject<BasketVM>(basketStr);

                foreach (BasketCookieItemVM cookie in basket.BasketCookieItemVMs.ToList())
                {
                    
                    Plant existed = await _context.Plants.Include(p => p.PlantImages).FirstOrDefaultAsync(p => p.Id == cookie.Id);

                    if (existed == null)
                    {
                        basket.BasketCookieItemVMs.Remove(cookie);
                        continue;
                    }
                    BasketItemVM basketItemVM = new BasketItemVM
                    {
                        Plant = existed,
                        Quantity = cookie.Quantity
                    };
                    layoutBasket.BasketItemVMs.Add(basketItemVM);
                }
                layoutBasket.TotalPrice = basket.TotalPrice;
            }

            return layoutBasket;
        }


    }

}
