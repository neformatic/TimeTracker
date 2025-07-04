using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeTracker.Common.Constants;
using TimeTracker.DAL.Constants;
using TimeTracker.DAL.Entities;
using TimeTracker.DAL.Seeds;

namespace TimeTracker.DAL.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRoleEntity>
{
    public void Configure(EntityTypeBuilder<UserRoleEntity> builder)
    {
        builder.ToTable(DatabaseTableNameConstants.UserRoles);

        builder.HasKey(ur => ur.Id);

        builder.Property(ur => ur.Description)
            .HasMaxLength(FieldSizeConstants.EnumDescriptionMaxSize);

        builder.HasData(UserRoleSeed.UserRoles);
    }
}