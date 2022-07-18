using Backend_Homework_Pronia.DAL;
using Backend_Homework_Pronia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_Homework_Pronia.ViewComponents
{
    public class FooterViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public FooterViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Setting> model = await _context.Settings.ToListAsync();
            return View(model);
        }
    }
}
