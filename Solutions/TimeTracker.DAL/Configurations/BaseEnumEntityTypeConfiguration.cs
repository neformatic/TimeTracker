using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeTracker.Common.Constants;
using TimeTracker.DAL.Entities;
using TimeTracker.DAL.Seeds;

namespace TimeTracker.DAL.Configurations;

public abstract class BaseEnumEntityTypeConfiguration<T, TK> : IEntityTypeConfiguration<T>
    where T : BaseEnumEntity<TK>, new() where TK : struct, Enum
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Description)
            .HasMaxLength(FieldSizeConstants.EnumDescriptionMaxSize)
            .IsRequired();

        builder.HasData(SeedData);

        AdditionalConfigure(builder);
    }

    protected virtual IEnumerable<T> SeedData { get; } = BaseEnumEntitySeed.GetItems<T, TK>();

    protected abstract void AdditionalConfigure(EntityTypeBuilder<T> builder);
}