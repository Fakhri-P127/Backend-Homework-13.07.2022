using Backend_Homework_Pronia.DAL;
using Backend_Homework_Pronia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_Homework_Pronia.Controllers
{
    public class PlantController:Controller
    {
        private readonly ApplicationDbContext _context;

        public PlantController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return NotFound();

            Plant plant = await _context.Plants.Include(p=>p.PlantImages)
                .Include(p=>p.PlantInformation).Include(p=>p.PlantColors)
                .Include(p=>p.PlantSizes).Include(p=>p.PlantCategories)
                .ThenInclude(p=>p.Category).Include(p=>p.PlantColors).ThenInclude(p=>p.Color)
                .Include(p=>p.PlantSizes).ThenInclude(p=>p.Size).
                Include(p=>p.PlantTags).ThenInclude(p=>p.Tag)
                .FirstOrDefaultAsync(p=>p.Id==id);
            if (plant is null) return NotFound();
            
            return View(plant);
        }
    }


}
