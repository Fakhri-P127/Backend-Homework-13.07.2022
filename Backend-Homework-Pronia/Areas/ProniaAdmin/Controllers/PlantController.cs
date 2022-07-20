using Backend_Homework_Pronia.DAL;
using Backend_Homework_Pronia.Extensions;
using Backend_Homework_Pronia.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_Homework_Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class PlantController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PlantController(ApplicationDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Plant> model = _context.Plants.Include(p => p.PlantInformation)
                .Include(p => p.PlantCategories).ThenInclude(p => p.Category)
                .Include(p => p.PlantImages).ToList();

            return View(model);
        }
        
        public IActionResult Create()
        {
            ViewBag.Information = _context.PlantInformations.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Plant plant)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Information = _context.PlantInformations.ToList();
                ViewBag.Categories = _context.Categories.ToList();
                return View();
            }
            if (plant.MainPhoto is null || plant.HoverPhoto is null || plant.DetailPhotos is null)
            {
                ViewBag.Information = _context.PlantInformations.ToList();
                ViewBag.Categories = _context.Categories.ToList();
                ModelState.AddModelError(string.Empty, "Choose at least 1 main image, 1 hover image and 1 detail image");
                return View();
            }

            if (!plant.MainPhoto.IsImageOkay(2) || !plant.HoverPhoto.IsImageOkay(2))
            {
                ViewBag.Information = _context.PlantInformations.ToList();
                ViewBag.Categories = _context.Categories.ToList();
                ModelState.AddModelError(string.Empty, "Please choose valid image file");
                return View();
            }

            TempData["FileName"] = "";
            List<PlantImage> DetailImages = new List<PlantImage>();

            foreach (IFormFile photo in plant.DetailPhotos.ToList())
            {
                if (!photo.IsImageOkay(2))
                {
                    plant.DetailPhotos.Remove(photo);
                    TempData["FileName"] += photo.FileName + ",";
                    continue;
                }
                PlantImage photoImage = new PlantImage
                {
                    Name = await photo.FileCreate(_env.WebRootPath, "assets/images/website-images"),
                    IsMain = false,
                    Alternative = plant.Name,
                    Plant = plant,
                    
                    
                };
                DetailImages.Add(photoImage);

            }
            // if all the photos are removed
            if (plant.DetailPhotos.Count == 0)
            {
                ViewBag.Information = _context.PlantInformations.ToList();
                ViewBag.Categories = _context.Categories.ToList();
                ModelState.AddModelError(string.Empty, "Couldn't load any of the detail images");
                return View();
            }

            PlantImage main = new PlantImage
            {
                Name = await plant.MainPhoto.FileCreate(_env.WebRootPath, "assets/images/website-images"),
                IsMain=true,
                Alternative=plant.Name,
                Plant=plant,
                
            };
            PlantImage hover = new PlantImage
            {
                Name = await plant.HoverPhoto.FileCreate(_env.WebRootPath, "assets/images/website-images"),
                IsMain = null,
                Alternative = plant.Name,
                Plant = plant
            };

            await _context.PlantImages.AddAsync(main);
            await _context.PlantImages.AddAsync(hover);
            await _context.PlantImages.AddRangeAsync(DetailImages);

            List<PlantCategory> pcategories = new List<PlantCategory>();
            foreach (var pcategoryId in plant.CategoryIds)
            {
                PlantCategory pcategory = new PlantCategory
                {
                    CategoryId = pcategoryId,
                    Plant=plant,
                };
                //burda birdefelik _context ede elave etmek olar amma nese ayri etmek istedim
                pcategories.Add(pcategory);
            }

            await _context.PlantCategories.AddRangeAsync(pcategories);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int? id)
        {
            if (id is null || id == 0) return NotFound();

            Plant existed = _context.Plants.Include(p => p.PlantInformation)
                .Include(p => p.PlantCategories).ThenInclude(p => p.Category)
                .Include(p => p.PlantImages).FirstOrDefault(p=>p.Id==id); 

            if (existed is null) return NotFound();

            ViewBag.Information = _context.PlantInformations.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            
            return View(existed);
        }
        
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int? id, Plant newPlant)
        {
            if (!ModelState.IsValid) 
            {
                ViewBag.Information = _context.PlantInformations.ToList();
                ViewBag.Categories = _context.Categories.ToList();
                ModelState.AddModelError(string.Empty, "Couldn't load any ");
                return View();
            };

            Plant currPlant = await _context.Plants.Include(p => p.PlantInformation)
                .Include(p => p.PlantCategories).ThenInclude(p => p.Category)
                .Include(p => p.PlantImages).FirstOrDefaultAsync(p => p.Id == id);

            if (currPlant is null) return NotFound();

            //_context.Entry(currPlant).CurrentValues.SetValues(newPlant);



            //foreach (var item in currPlant.PlantImages.Where(p=>p.IsMain==false).ToList())
            //{
            //    currPlant.PlantImages.RemoveAll(p => p.Name == newPlant.Name);
            //    //FileValidator.FileDelete(_env.WebRootPath, "assets/images/website-images", item.Name);

            //    //item.Name = await newPlant.DetailPhotos.FirstOrDefault().FileCreate(_env.WebRootPath, "assets/images/website-images");

            //    //foreach (var image in currPlant.DetailPhotos)
            //    //{

            //    //    FileValidator.FileDelete(_env.WebRootPath, "assets/images/website-images", item.Name);
            //    //    item.Name = await image.FileCreate(_env.WebRootPath, "assets/images/website-images");
            //    //}
            //}

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

       
    }
}
