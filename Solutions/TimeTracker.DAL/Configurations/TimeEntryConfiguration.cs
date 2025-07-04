using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeTracker.DAL.Entities;

namespace TimeTracker.DAL.Configurations;

public class TimeEntryConfiguration : IEntityTypeConfiguration<TimeEntry>
{
    public void Configure(EntityTypeBuilder<TimeEntry> builder)
    {
        builder.HasKey(te => te.Id);

        builder.HasOne(te => te.User)
            .WithMany(u => u.TimeEntries)
            .HasForeignKey(te => te.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(te => te.StartTime).IsRequired();
    }
}