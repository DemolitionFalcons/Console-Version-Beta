namespace DemolitionFalcons.Data.EntityConfigurations
{
    using DemolitionFalcons.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    public class SpellConfig : IEntityTypeConfiguration<Spell>
    {

        public void Configure(EntityTypeBuilder<Spell> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .HasMaxLength(20)
                .IsRequired();
        }
    }
}
