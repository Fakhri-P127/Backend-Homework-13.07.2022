using Backend_Homework_Pronia.DAL;
using Backend_Homework_Pronia.Extensions;
using Backend_Homework_Pronia.Models;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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
            if (plant.MainPhoto is null || plant.HoverPhoto is null
                || plant.DetailPhotos is null)
            {
                return ErrorMessage(
                    "Choose at least 1 main image, 1 hover image and 1 detail image");
            }

            if (!plant.MainPhoto.IsImageOkay(2) || !plant.HoverPhoto.IsImageOkay(2))
            {
                return ErrorMessage("Please choose valid image file");
             
            }

            TempData["FileName"] = default(string);
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
                    Name = await photo.FileCreate(_env.WebRootPath,
                    "assets/images/website-images", null, _context.PlantImages),
                    IsMain = false,
                    Alternative = plant.Name,
                    Plant = plant,
                };
                DetailImages.Add(photoImage);

            }
            // if all the photos are removed
            if (plant.DetailPhotos.Count == 0)
            {
                return ErrorMessage("Couldn't load any of the detail images");
            }

            PlantImage main = new PlantImage
            {
                Name = await plant.MainPhoto.FileCreate(_env.WebRootPath,
                "assets/images/website-images", null, _context.PlantImages),
                IsMain =true,
                Alternative=plant.Name,
                Plant=plant,
            };
            PlantImage hover = new PlantImage
            {
                Name = await plant.HoverPhoto.FileCreate(_env.WebRootPath,
                "assets/images/website-images", null, _context.PlantImages),
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

        [Authorize(Roles = "Admin,Moderator")]
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

            Plant currPlant = await _context.Plants
                .Include(p => p.PlantImages)
                .Include(p => p.PlantInformation)
                .Include(p => p.PlantCategories)
                .ThenInclude(p => p.Category).SingleOrDefaultAsync(p => p.Id == id);
           
            if (currPlant is null) return NotFound();

            if (!ModelState.IsValid)
            {
                return ErrorMessage("Couldn't load any",currPlant);
            };

            if (newPlant.ImageIds is null && newPlant.DetailPhotos is null)
            {
                return ErrorMessage("You must upload at least 1 detail image",currPlant);
            }
            if (newPlant.CategoryIds is null)
            {
                return ErrorMessage("You must choose at least 1 category",currPlant);
            }

            _context.Entry(currPlant).CurrentValues.SetValues(newPlant);

            //if the user inputs detail photos
            TempData["FileName"] = default(string);

            if (newPlant.DetailPhotos != null)
            {
                // butun detail shekillerini silib bashqalarini elave etmek uchundu
                if (newPlant.ImageIds is null)
                {
                    foreach (PlantImage dtlImage in currPlant.PlantImages
                        .Where(p => p.IsMain == false))
                    {
                        FileValidator.FileDelete(_env.WebRootPath,
                            "assets/images/website-images", dtlImage.Name);
                    }
                    currPlant.PlantImages.RemoveAll(p => p.IsMain == false);
                }
                else
                {
                    List<PlantImage> removableImages = currPlant.PlantImages
                  .Where(p => p.IsMain == false && !newPlant.ImageIds
                  .Contains(p.Id)).ToList();

                    currPlant.PlantImages
                        .RemoveAll(p => removableImages.Any(r => p.Id == r.Id));

                    removableImages.ForEach(r => FileValidator.FileDelete(_env.WebRootPath,
                        "assets/images/website-images", r.Name));

                }

                foreach (IFormFile image in newPlant.DetailPhotos.ToList())
                {
                    //butun upload olunmush shekiller shertlerimize uymursa
                    if (newPlant.DetailPhotos.Count == 0)
                    {
                        return ErrorMessage(
                            "None of the detail images are valid type",currPlant);
                    }
                    // burda yuklediyimiz bir neche shekilden her hansisa problemli olsa silsin deyirik. Bir iki shekile gore butun shekiller silinmesin
                    if (!image.IsImageOkay(2))
                    {
                        newPlant.DetailPhotos.Remove(image);
                        TempData["FileName"] += image.FileName + ",";
                        continue;
                    }

                    PlantImage detailImage = new PlantImage
                    {
                        Name = await image.FileCreate(_env.WebRootPath,
                        "assets/images/website-images", null, _context.PlantImages),
                        IsMain = false,
                        Plant = currPlant,
                        Alternative = newPlant.Name,
                    };
                    await _context.PlantImages.AddAsync(detailImage);
                }
            }

            //if the user inputs main photo
            if (newPlant.MainPhoto != null)
            {
                if (!newPlant.MainPhoto.IsImageOkay(2))
                {
                    return ErrorMessage("Please choose valid image file",currPlant);
                }
                PlantImage main = new PlantImage
                {
                    IsMain = true,
                    Alternative = newPlant.Name,
                    Name = await newPlant.MainPhoto
                    .FileCreate(_env.WebRootPath,
                    "assets/images/website-images", null, _context.PlantImages),
                    Plant = currPlant,
                };
                string mainPhoto = currPlant.PlantImages
                    .FirstOrDefault(p => p.IsMain == true).Name;
                FileValidator.FileDelete(_env.WebRootPath, 
                    "assets/images/website-images", mainPhoto);
                //name ve alternative i yaratdigimiza menimsedirik ki yeni photo ile override edek
                currPlant.PlantImages.FirstOrDefault(p => p.IsMain == true).Name = main.Name;
                currPlant.PlantImages.FirstOrDefault(p => p.IsMain == true)
                    .Alternative = main.Alternative;
            }

            //if the user inputs hover photo
            if (newPlant.HoverPhoto != null)
            {
                if (!newPlant.HoverPhoto.IsImageOkay(2))
                {
                   return ErrorMessage("Please choose valid image file",currPlant);
                }
                PlantImage hover = new PlantImage
                {
                    IsMain = null,
                    Alternative = newPlant.Name,
                    Name = await newPlant.HoverPhoto.FileCreate(_env.WebRootPath,
                    "assets/images/website-images", null, _context.PlantImages),
                    Plant = currPlant
                };
                string filename = currPlant.PlantImages
                    .FirstOrDefault(p => p.IsMain == null).Name;
                //kohne shekili silirik
                FileValidator.FileDelete(_env.WebRootPath,
                    "assets/images/website-images", filename);
                //name ve alternative i yaratdigimiza menimsedirik ki yeni photo ile override edek
                currPlant.PlantImages
                    .FirstOrDefault(p => p.IsMain == null).Name = hover.Name;
                currPlant.PlantImages
                    .FirstOrDefault(p => p.IsMain == null).Alternative = hover.Alternative;
            }

            // Editing Category starts
            //removing categories that doesn't match with CategoryIds
            List<PlantCategory> removableCategories = currPlant.PlantCategories
                .Where(p => !newPlant.CategoryIds.Contains(p.CategoryId)).ToList();

            currPlant.PlantCategories.RemoveAll(c => removableCategories
            .Any(r => r.CategoryId == c.CategoryId));

            //adding categories that don't contain CategoryIds
            newPlant.CategoryIds.ForEach(async ctgId =>
            {
                PlantCategory currCategory = currPlant.PlantCategories
                .FirstOrDefault(p => p.CategoryId == ctgId);

                if (!currPlant.PlantCategories.Contains(currCategory))
                {
                    PlantCategory category = new PlantCategory
                    {
                        CategoryId = ctgId,
                        Plant = currPlant
                    };
                    await _context.PlantCategories.AddAsync(category);
                }
            });
            // Editing Category ends

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(int? id)
        {
            if (id is null || id == 0) return NotFound();

            Plant currPlant = await _context.Plants.Include(p=>p.PlantImages).FirstOrDefaultAsync(p => p.Id == id);
            currPlant.PlantImages.ForEach(pImage => FileValidator.FileDelete(_env.WebRootPath,
                "assets/images/website-images", pImage.Name));
            
            _context.Plants.Remove(currPlant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        // Utility methods
        public IActionResult ErrorMessage(string text, Plant currPlant=null)
        {
            ViewBag.Information = _context.PlantInformations.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ModelState.AddModelError(string.Empty, text);
            return View(currPlant);
        }
    }
}
