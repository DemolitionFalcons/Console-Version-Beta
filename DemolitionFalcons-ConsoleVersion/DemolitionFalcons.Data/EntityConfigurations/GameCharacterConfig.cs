namespace DemolitionFalcons.Data.EntityConfigurations
{
    using DemolitionFalcons.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class GameCharacterConfig : IEntityTypeConfiguration<GameCharacter>
    {
        public void Configure(EntityTypeBuilder<GameCharacter> builder)
        {
            builder.HasKey(z => new { z.GameId, z.CharacterId });

            builder.HasOne(g => g.Game)
                .WithMany(ch => ch.Characters)
                .HasForeignKey(g => g.GameId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ch => ch.Character)
                .WithMany(g => g.Games)
                .HasForeignKey(ch => ch.CharacterId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
