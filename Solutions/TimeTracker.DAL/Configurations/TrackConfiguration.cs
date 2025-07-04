using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeTracker.DAL.Entities;

namespace TimeTracker.DAL.Configurations;

public class TrackConfiguration : IEntityTypeConfiguration<Track>
{
    public void Configure(EntityTypeBuilder<Track> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Title).IsRequired().HasMaxLength(255);
        builder.Property(t => t.Artist).IsRequired().HasMaxLength(255);
        builder.Property(t => t.Genre).HasMaxLength(100);
        builder.Property(t => t.FilePath).IsRequired();
    }
}