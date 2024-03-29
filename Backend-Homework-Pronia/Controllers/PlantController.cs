﻿using Backend_Homework_Pronia.DAL;
using Backend_Homework_Pronia.Models;
using Backend_Homework_Pronia.ViewModels;
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
    public class PlantController:Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public PlantController(ApplicationDbContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null || id == 0) return NotFound();

            PlantVM model = new PlantVM
            {
                Plant = await _context.Plants.Include(p => p.PlantImages)
                .Include(p => p.PlantInformation).Include(p => p.PlantColors)
                .Include(p => p.PlantCategories)
                .ThenInclude(p => p.Category).Include(p => p.PlantColors).ThenInclude(p => p.Color)
                .ThenInclude(p=>p.ColorSizes).ThenInclude(p=>p.Size)
                .Include(p => p.PlantTags).ThenInclude(p => p.Tag)
                .FirstOrDefaultAsync(p => p.Id == id),
                Plants= new List<Plant>()
            };
            
            if (model.Plant is null) return NotFound();
            List<Plant> plants = new List<Plant>();
            foreach (PlantCategory pCategory in model.Plant.PlantCategories)
            {

                plants = await _context.Plants.Include(x => x.PlantCategories)
                    .Include(x => x.PlantImages)
                    .Where(p => p.PlantCategories
                    .Any(x => x.CategoryId == pCategory.CategoryId) && p.Id != pCategory.PlantId).ToListAsync();

                model.Plants.AddRange(plants);
            }
            model.Plants = model.Plants.Distinct().ToList();
            return View(model);
        }

        public async Task<IActionResult> Partial()
        {
            List<Plant> plants = await _context.Plants.Include(p => p.PlantImages).ToListAsync();

            return PartialView("_PlantsPartialView",plants.OrderByDescending(p=>p.Id).Take(4));
        }

        public async Task<IActionResult> AddBasket(int? id)
        {
            if (id is null || id == 0) return NotFound();


            Plant plant = await _context.Plants.FirstOrDefaultAsync(p => p.Id == id);

            if (plant is null) return NotFound();

          

            if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

                if (user is null) return NotFound();
                user.BasketItems = await _context.BasketItems
                    .Include(b=>b.Plant).Include(b=>b.AppUser).Where(b => b.AppUserId == user.Id).ToListAsync();

                BasketItem existed = await _context.BasketItems.FirstOrDefaultAsync(b => b.PlantId == plant.Id && user.Id == b.AppUserId);
                if(existed is null)
                {
                    existed = new BasketItem
                    {
                        AppUser = user,
                        Plant=plant,
                        Quantity=1,
                        Price=plant.Price
                    };
                    user.BasketItems.Add(existed);
                }
                else
                {
                    existed.Quantity++;
                }
                await _context.SaveChangesAsync();
            }
            else
            {
                string basketStr = HttpContext.Request.Cookies["Basket"];
                BasketVM basket;

                if (string.IsNullOrEmpty(basketStr))
                {
                    basket = new BasketVM();
                    BasketCookieItemVM basketCookieItem = new BasketCookieItemVM
                    {
                        Id = plant.Id,
                        Quantity = 1
                    };
                    basket.BasketCookieItemVMs = new List<BasketCookieItemVM>();
                    basket.BasketCookieItemVMs.Add(basketCookieItem);
                    basket.TotalPrice = plant.Price;
                }
                else
                {
                    basket = JsonConvert.DeserializeObject<BasketVM>(basketStr);
                    BasketCookieItemVM existed = basket.BasketCookieItemVMs.FirstOrDefault(p => p.Id == id);

                    if (existed is null)
                    {
                        BasketCookieItemVM basketCookieItem = new BasketCookieItemVM
                        {
                            Id = plant.Id,
                            Quantity = 1
                        };

                        basket.BasketCookieItemVMs.Add(basketCookieItem);
                        basket.TotalPrice += plant.Price;
                    }
                    else
                    {
                        basket.TotalPrice += plant.Price;
                        existed.Quantity++;
                    }
                }

                basketStr = JsonConvert.SerializeObject(basket);
                HttpContext.Response.Cookies.Append("Basket", basketStr);
            }
            return RedirectToAction("Index","Home");
        }

        public async Task<IActionResult> RemoveItemFromBasket(int? id)
        {
            if (id is null || id == 0) return NotFound();

            Plant plant = await _context.Plants.FirstOrDefaultAsync(p => p.Id == id);

            if (plant is null) return NotFound();

            string basketStr = HttpContext.Request.Cookies["Basket"];

            if (string.IsNullOrEmpty(basketStr))
            {
                //Eger datadan bu mal silinse amma yenede basketde qalibsa o vaxt NotFound() versin
                return NotFound();
            }
            BasketVM basket = JsonConvert.DeserializeObject<BasketVM>(basketStr);
          
            BasketCookieItemVM existed = basket.BasketCookieItemVMs.FirstOrDefault(p => p.Id == id);
            basket.BasketCookieItemVMs.Remove(existed);
          
            basketStr = JsonConvert.SerializeObject(basket);
            HttpContext.Response.Cookies.Append("Basket", basketStr);

            return RedirectToAction("Index", "Home");
        }
        public IActionResult ShowBasket()
        {
            if (HttpContext.Request.Cookies["Basket"] is null) return NotFound();
            BasketVM basket = JsonConvert.DeserializeObject<BasketVM>(HttpContext.Request.Cookies["Basket"]);
            return Json(basket);
        }
        public async Task<IActionResult> GetDatas(int? id)
        {
            Plant plant = await _context.Plants.Include(p => p.PlantImages).Include(p => p.PlantInformation).FirstOrDefaultAsync(p => p.Id == id);
            return PartialView("_PlantsFetchPartialView",plant);
        }
    }


}
