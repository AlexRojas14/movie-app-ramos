using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieAppRamos.Domain.Entities;

namespace MovieAppRamos.Infrastructure.Persistence.Configurations;

public sealed class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable("Movies");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(Movie.MaxTitleLength);

        builder.Property(x => x.Description)
            .HasMaxLength(Movie.MaxDescriptionLength);

        builder.Property(x => x.ReleaseDate)
            .HasColumnType("date");

        builder.Property(x => x.IsDisabled)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasIndex(x => x.Title)
            .HasDatabaseName("IX_Movies_Title");

        builder.HasIndex(x => new { x.IsDisabled, x.Title })
            .HasDatabaseName("IX_Movies_IsDisabled_Title");

        builder.HasMany(x => x.Reviews)
            .WithOne()
            .HasForeignKey(x => x.MovieId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(x => x.Reviews)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

