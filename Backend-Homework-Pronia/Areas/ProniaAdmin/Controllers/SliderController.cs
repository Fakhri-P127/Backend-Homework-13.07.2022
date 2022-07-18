using Backend_Homework_Pronia.DAL;
using Backend_Homework_Pronia.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_Homework_Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class SliderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SliderController(ApplicationDbContext context)
        {
            _context = context;
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



            return RedirectToAction(nameof(Index));      
        }
    }
}
