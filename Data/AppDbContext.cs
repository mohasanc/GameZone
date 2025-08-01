﻿using GameZone.Models;

namespace GameZone.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<GameDevice> GameDevices { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasData(new List<Category>
                {
                    new Category{Id = 1, Name = "Sports"},
                    new Category{Id = 2, Name = "Action"},
                    new Category{Id = 3, Name = "Adventure"},
                    new Category{Id = 4, Name = "Racing"},
                    new Category{Id = 5, Name = "Fighting"},
                    new Category{Id = 6, Name = "Film"}
                });

            modelBuilder.Entity<Device>()
                .HasData(new List<Device>
                {
                    new Device {Id = 1, Name = "PlayStation", Icon = "bi bi-playstation"},
                    new Device {Id = 2, Name = "XBox", Icon = "bi bi-xbox"},
                    new Device {Id = 3, Name = "Nintendo Switch", Icon = "bi bi-nintendo-switch"},
                    new Device {Id = 4, Name = "PC", Icon = "bi bi-pc-display"}
                });

            modelBuilder.Entity<GameDevice>()
                .HasKey(e => new { e.GameId, e.DeviceId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
