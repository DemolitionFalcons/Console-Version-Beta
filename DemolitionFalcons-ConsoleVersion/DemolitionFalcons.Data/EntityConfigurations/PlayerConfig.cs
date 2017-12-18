namespace DemolitionFalcons.Data.EntityConfigurations
{
    using DemolitionFalcons.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PlayerConfig : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Username)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(p => p.Password)
                .HasMaxLength(20)
                .IsRequired();

            builder.HasAlternateKey(p => p.Username);

            //builder.HasMany(p => p.Characters);
        }
    }
}
