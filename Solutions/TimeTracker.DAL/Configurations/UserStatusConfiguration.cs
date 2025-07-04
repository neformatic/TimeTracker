using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeTracker.Common.Enums;
using TimeTracker.DAL.Constants;
using TimeTracker.DAL.Entities;

namespace TimeTracker.DAL.Configurations;

public class UserStatusConfiguration : BaseEnumEntityTypeConfiguration<UserStatusEntity, UserStatus>
{
    protected override void AdditionalConfigure(EntityTypeBuilder<UserStatusEntity> builder)
    {
        builder.ToTable(DatabaseTableNameConstants.UserStatuses);
    }
}