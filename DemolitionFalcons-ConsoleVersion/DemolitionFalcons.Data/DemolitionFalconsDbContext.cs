namespace DemolitionFalcons.Data
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using DemolitionFalcons.Data.EntityConfigurations;
    using DemolitionFalcons.Models;

    public class DemolitionFalconsDbContext : DbContext
    {
        public DemolitionFalconsDbContext(DbContextOptions options) : base(options)
        {
        }

        public DemolitionFalconsDbContext()
        {
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<PlayerWeapon> PlayerWeapons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PlayerConfig());
            modelBuilder.ApplyConfiguration(new WeaponConfig());
            modelBuilder.ApplyConfiguration(new PlayerWeaponConfig());
        }
    }
}
