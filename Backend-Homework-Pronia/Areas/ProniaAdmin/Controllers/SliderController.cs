using Backend_Homework_Pronia.DAL;
using Backend_Homework_Pronia.Extensions;
using Backend_Homework_Pronia.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_Homework_Pronia.Areas.ProniaAdmin.Controllers
{

    [Area("ProniaAdmin")]
    public class SliderController : Controller
    {
        

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(ApplicationDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Slider> slider = _context.Sliders.ToList();
            return View(slider);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (!ModelState.IsValid) return View();

            if (!slider.Photo.IsImageOkay(3))
            {
                ModelState.AddModelError("Photo", "Please choose valid image file");
                return View();
            }

           
            // _context gonderirem cunki eyni adda image olmasini yoxlamaq ucun lazimdi ve extension klasi static oldugu ucun _context ishletmek olmur
            slider.Image = await slider.Photo.FileCreate (_env.WebRootPath,"assets/images/slider");
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));      
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null || id == 0) return NotFound();

            Slider existed = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (existed is null) return NotFound();

            return View(existed);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int? id, Slider newSlider)
        {
            Slider currSlider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (currSlider is null) return NotFound();

            if(newSlider.Photo is null)
            {
                string filename = currSlider.Image;
                _context.Entry(currSlider).CurrentValues.SetValues(newSlider);
                currSlider.Image = filename;
            }
            else
            {
                if (!newSlider.Photo.IsImageOkay(3))
                {
                    ModelState.AddModelError("Photo", "Please choose valid image file");
                    return View(currSlider);
                }

                FileValidator.FileDelete(_env.WebRootPath, "assets/images/slider", currSlider.Image);
                _context.Entry(currSlider).CurrentValues.SetValues(newSlider);
                currSlider.Image = await newSlider.Photo.FileCreate(_env.WebRootPath, "assets/images/slider");

            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int? id)
        {
            if (id is null || id == 0) return NotFound();

            Slider currSlider = await _context.Sliders.FirstOrDefaultAsync(p => p.Id == id);
            if (currSlider is null) return NotFound();

             _context.Sliders.Remove(currSlider);
            FileValidator.FileDelete(_env.WebRootPath, "assets/images/slider", currSlider.Image);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
