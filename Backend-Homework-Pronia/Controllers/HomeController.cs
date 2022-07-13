using Backend_Homework_Pronia.DAL;
using Backend_Homework_Pronia.Models;
using Backend_Homework_Pronia.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_Homework_Pronia.Controllers
{
    public class HomeController:Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeVM model = new HomeVM
            {
                Sliders = _context.Sliders.ToList(),
                Plants = _context.Plants.Include(p => p.PlantImages).ToList()
            };
            
            return View(model);

        }
    }
}
