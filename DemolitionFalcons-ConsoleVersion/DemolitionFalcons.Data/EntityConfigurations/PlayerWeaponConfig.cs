namespace DemolitionFalcons.Data.EntityConfigurations
{
    using DemolitionFalcons.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PlayerWeaponConfig : IEntityTypeConfiguration<PlayerWeapon>
    {
        public void Configure(EntityTypeBuilder<PlayerWeapon> builder)
        {
            builder.HasKey(z => new { z.PlayerId, z.WeaponId });

            builder.HasOne(z => z.Player)
                .WithMany(p => p.Weapons)
                .HasForeignKey(z => z.PlayerId);

            builder.HasOne(z => z.Weapon)
                .WithMany(w => w.Players)
                .HasForeignKey(z => z.WeaponId);
        }
    }
}
