using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieAppRamos.Domain.Entities;

namespace MovieAppRamos.Infrastructure.Persistence.Configurations;

public sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews", tableBuilder =>
        {
            tableBuilder.HasCheckConstraint(
                "CK_Reviews_Rating",
                $"[Rating] >= {Review.MinRating} AND [Rating] <= {Review.MaxRating}");
        });

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.MovieId).IsRequired();

        builder.Property(x => x.AuthorName)
            .IsRequired()
            .HasMaxLength(Review.MaxAuthorNameLength);

        builder.Property(x => x.Rating).IsRequired();

        builder.Property(x => x.Comment)
            .HasMaxLength(Review.MaxCommentLength);

        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasIndex(x => x.MovieId)
            .HasDatabaseName("IX_Reviews_MovieId");

        builder.HasIndex(x => new { x.MovieId, x.CreatedAt })
            .HasDatabaseName("IX_Reviews_MovieId_CreatedAt");
    }
}

