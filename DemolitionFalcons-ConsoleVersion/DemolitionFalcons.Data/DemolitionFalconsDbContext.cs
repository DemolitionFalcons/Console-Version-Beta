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

        protected DemolitionFalconsDbContext()
        {
        }

        public DbSet<Character> Characters { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CharacterConfig());
        }
    }
}
