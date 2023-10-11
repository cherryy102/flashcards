using Flashcards.Domain.Entities;
using Flashcards.Infrastructure.Persistence.Flashcards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flashcards.Infrastructure.Persistence.Configurations
{
    internal class SetsConf : IEntityTypeConfiguration<Set>
    {
        public void Configure(EntityTypeBuilder<Set> builder)
        {
            builder.ToTable("sets", FlashcardsDbContext.DefaultSchema);

            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Cards).WithOne(x => x.Set)
                .HasPrincipalKey(x => x.Id)
                .HasForeignKey(x => x.SetId);

            builder.Property(x => x.UserId)
                .HasColumnType("integer");

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.Name)
                .HasColumnName("name");

            builder.Property(x => x.UserId)
                .HasColumnType("uuid")
                .HasColumnName("user_id");

            builder.Property(x => x.DateAdd)
                .HasColumnName("date_add");
        }
    }
}
