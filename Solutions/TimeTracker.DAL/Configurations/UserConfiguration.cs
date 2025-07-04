using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeTracker.Common.Constants;
using TimeTracker.Common.Enums;
using TimeTracker.DAL.Constants;
using TimeTracker.DAL.Entities;
using TimeTracker.DAL.Seeds;

namespace TimeTracker.DAL.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(DatabaseTableNameConstants.Users);

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name)
            .HasMaxLength(FieldSizeConstants.UserNameMaxSize)
            .IsRequired();

        builder.Property(u => u.Password)
            .HasMaxLength(FieldSizeConstants.HashedPasswordMaxSize)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasColumnType(DatabaseColumnTypeConstants.CaseInsensitiveText)
            .IsRequired();

        builder.Property(u => u.UserRole)
            .HasColumnName(DatabaseColumnNameConstants.UserRoleId);

        builder.Property(u => u.Status)
            .HasColumnName(DatabaseColumnNameConstants.StatusId)
            .HasDefaultValue(UserStatus.Active);

        builder.Property(u => u.CreatedDateTime)
            .HasDefaultValueSql(DatabaseFunctionConstants.CurrentTimestamp);

        builder.Property(u => u.UpdatedDateTime)
            .HasDefaultValueSql(DatabaseFunctionConstants.CurrentTimestamp);

        builder.HasIndex(u => u.Email).IsUnique();

        builder.HasOne(u => u.UserStatusEntity)
            .WithMany(us => us.Users)
            .HasForeignKey(u => u.Status)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(u => u.UserRoleEntity)
            .WithMany(ur => ur.Users)
            .HasForeignKey(u => u.UserRole)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasData(UserSeed.Users);
    }
}