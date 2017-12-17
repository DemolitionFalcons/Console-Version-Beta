namespace DemolitionFalcons.Data.EntityConfigurations
{
    using DemolitionFalcons.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class WeaponConfig : IEntityTypeConfiguration<Weapon>
    {
        public void Configure(EntityTypeBuilder<Weapon> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.Name)
                .HasMaxLength(25)
                .IsRequired();



        }
    }
}
