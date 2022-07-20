using Backend_Homework_Pronia.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_Homework_Pronia.DAL
{
    public class ApplicationDbContext:IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<PlantImage> PlantImages { get; set; }
        public DbSet<PlantInformation> PlantInformations { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<PlantCategory> PlantCategories { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<PlantColor> PlantColors { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<PlantSize> PlantSizes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PlantTag> PlantTags { get; set; }
        public DbSet<Setting> Settings { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var item in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e=>e.GetProperties()
                .Where(p=>p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?))))
            {
                item.SetColumnType("decimal(6,2)");
            }
            modelBuilder.Entity<Setting>().HasIndex(p => p.Key).IsUnique();
            modelBuilder.Entity<Category>().HasIndex(p => p.Name).IsUnique();
          base.OnModelCreating(modelBuilder);
        }
    }
}
