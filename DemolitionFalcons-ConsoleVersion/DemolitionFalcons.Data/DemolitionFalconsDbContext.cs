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
        public DbSet<Game> Games { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<GameCharacter> GameCharacters { get; set; }
        public DbSet<Spell> Spells { get; set; }

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
            modelBuilder.ApplyConfiguration(new GameConfig());
            modelBuilder.ApplyConfiguration(new CharacterConfig());
            modelBuilder.ApplyConfiguration(new GameCharacterConfig());
            modelBuilder.ApplyConfiguration(new SpellConfig());
        }
    }
}
