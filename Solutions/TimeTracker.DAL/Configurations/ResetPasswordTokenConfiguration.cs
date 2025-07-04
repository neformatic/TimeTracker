using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeTracker.Common.Constants;
using TimeTracker.DAL.Constants;
using TimeTracker.DAL.Entities;

namespace TimeTracker.DAL.Configurations;

public class ResetPasswordTokenConfiguration : IEntityTypeConfiguration<ResetPasswordToken>
{
    public void Configure(EntityTypeBuilder<ResetPasswordToken> builder)
    {
        builder.ToTable(DatabaseTableNameConstants.ResetPasswordTokens);

        builder.HasKey(t => t.UserId);

        builder.Property(t => t.Token)
            .IsRequired()
            .HasMaxLength(FieldSizeConstants.PasswordTokenMaxSize);

        builder.Property(t => t.CreatedDateTime)
            .HasDefaultValueSql(DatabaseFunctionConstants.CurrentTimestamp);

        builder.HasOne(t => t.User)
            .WithOne(u => u.ResetPasswordToken)
            .HasForeignKey<ResetPasswordToken>(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}