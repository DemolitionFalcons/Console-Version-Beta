namespace DemolitionFalcons.Data.EntityConfigurations
{
    using DemolitionFalcons.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class GameConfig : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.HasKey(g => g.Id);

            builder.HasAlternateKey(g => g.Name);

            builder.Property(g => g.Name)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(g => g.Map)
                .IsRequired();
        }
    }
}
