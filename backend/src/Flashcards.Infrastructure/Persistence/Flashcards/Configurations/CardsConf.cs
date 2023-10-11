using Flashcards.Domain.Entities;
using Flashcards.Infrastructure.Persistence.Flashcards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flashcards.Infrastructure.Persistence.Configurations
{
    internal class CardsConf : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.ToTable("cards", FlashcardsDbContext.DefaultSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .HasColumnType("integer");

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.Definition)
                .HasColumnName("definition");

            builder.Property(x => x.Term)
                .HasColumnName("term");

            builder.Property(x => x.UserId)
                .HasColumnType("uuid")
                .HasColumnName("user_id");

            builder.Property(x => x.SetId)
                .HasColumnName("set_id");

            builder.Property(x => x.DateAdd)
                .HasColumnName("date_add");

            builder.Property(x => x.Correctness)
                .HasColumnName("correctness");

            builder.Property(x => x.Stage)
                .HasColumnName("stage");

            builder.Property(x => x.NextRepeatDate)
                .HasColumnName("next_repeat_date");
        }
    }
}
