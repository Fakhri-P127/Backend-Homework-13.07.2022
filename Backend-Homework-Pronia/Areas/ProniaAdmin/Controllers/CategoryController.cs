using Backend_Homework_Pronia.DAL;
using Backend_Homework_Pronia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_Homework_Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            List<Category> model = _context.Categories.ToList();
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid) return View();
            Category existed = _context.Categories.FirstOrDefault(x => x.Name.ToLower().Trim() == category.Name.ToLower().Trim());
                if(existed != null)
            {
                ModelState.AddModelError("Name", "That category already exists");
                return View();
            }
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int? id)
        {
            if (id is null || id == 0) return NotFound();

            Category category = _context.Categories.FirstOrDefault(x => x.Id == id);

            if (category is null) return NotFound();

            return View(category);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(int? id, Category newCategory)
        {
            if (id is null || id == 0) return NotFound();
            if (!ModelState.IsValid) return View();

            Category existed = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (existed is null) return BadRequest();

            bool isSameCategoryName = _context.Categories.Any(x => x.Name.ToLower().Trim() == newCategory.Name.ToLower().Trim());

            if (isSameCategoryName) 
            {
                ModelState.AddModelError("Name", "Category with this name already exists");
                return View();
            }



            _context.Entry(existed).CurrentValues.SetValues(newCategory);

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Remove(int? id)
        {
            if (id is null || id == 0) return NotFound();

            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category is null) return NotFound();
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }
    }
}
