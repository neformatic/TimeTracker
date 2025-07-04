using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeTracker.Common.Constants;
using TimeTracker.DAL.Constants;
using TimeTracker.DAL.Entities;

namespace TimeTracker.DAL.Configurations;

public class UserRefreshTokenConfiguration : IEntityTypeConfiguration<UserRefreshToken>
{
    public void Configure(EntityTypeBuilder<UserRefreshToken> entity)
    {
        entity.ToTable(DatabaseTableNameConstants.UserRefreshTokens);

        entity.HasKey(u => u.Id);

        entity.Property(e => e.RefreshToken)
            .IsRequired()
            .HasMaxLength(FieldSizeConstants.RefreshTokenMaxSize);

        entity.HasOne(t => t.User)
            .WithMany(t => t.UserRefreshTokens)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(e => new
        {
            e.UserId,
            e.RefreshToken
        })
            .IsUnique();
    }
}