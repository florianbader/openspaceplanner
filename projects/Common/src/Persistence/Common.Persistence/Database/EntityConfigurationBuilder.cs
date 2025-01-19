using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using RioScaffolding.OpenSpacePlanner.Common.Persistence.EntityConfigurations;

namespace RioScaffolding.OpenSpacePlanner.Common.Persistence.Database;

public static class EntityConfigurationBuilder
{
    public static KeyBuilder IsGuidKey<TEntity>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, object?>> keyExpression,
        Expression<Func<TEntity, object?>>? clusteredIndex = null
    )
        where TEntity : class
    {
        if (clusteredIndex is not null)
        {
            builder.HasClusteredIndex(clusteredIndex);
        }

        builder.Property(keyExpression).ValueGeneratedOnAdd().HasValueGenerator<SequentialGuidValueGenerator>();

        return builder.HasKey(keyExpression).HasAnnotation("SqlServer:Clustered", clusteredIndex is null);
    }

    public static KeyBuilder IsKey<TEntity>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, object?>> keyExpression,
        bool isClustered = true
    )
        where TEntity : class
    {
        builder.Property(keyExpression).ValueGeneratedOnAdd();
        return builder.HasKey(keyExpression).HasAnnotation("SqlServer:Clustered", isClustered);
    }

    public static KeyBuilder IsKey<TEntity>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, object?>> keyExpression,
        Expression<Func<TEntity, object?>> clusteredIndex
    )
        where TEntity : class
    {
        builder.HasClusteredIndex(clusteredIndex);
        return builder.IsKey(keyExpression, false);
    }

    public static KeyBuilder IsKey<TEntity>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, object?>> keyExpression,
        string[] clusteredIndexProperties
    )
        where TEntity : class
    {
        builder.HasClusteredIndex(clusteredIndexProperties);
        return builder.IsKey(keyExpression, false);
    }

    public static IndexBuilder<TEntity> HasClusteredIndex<TEntity>(
        this EntityTypeBuilder<TEntity> builder,
        params string[] clusteredIndexProperties
    )
        where TEntity : class => builder.HasIndex(clusteredIndexProperties).IsUnique().IsClustered();

    public static IndexBuilder<TEntity> HasClusteredIndex<TEntity>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, object?>> clusteredIndex
    )
        where TEntity : class => builder.HasIndex(clusteredIndex).IsUnique().IsClustered();

    public static IndexBuilder<TEntity> IsUnique<TEntity>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, object?>> propertyExpression
    )
        where TEntity : class => builder.HasIndex(propertyExpression).IsUnique();

    public static PropertyBuilder<TProperty> IsIdentifierText<TEntity, TProperty>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, TProperty>> propertyExpression
    )
        where TEntity : class => builder.HasText(propertyExpression, DataTypes.IdentifierLength);

    public static PropertyBuilder<TProperty> IsShortText<TEntity, TProperty>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, TProperty>> propertyExpression
    )
        where TEntity : class => builder.HasText(propertyExpression, DataTypes.ShortTextLength);

    public static PropertyBuilder<TProperty> IsLongText<TEntity, TProperty>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, TProperty>> propertyExpression
    )
        where TEntity : class => builder.HasText(propertyExpression, DataTypes.LongTextLength);

    private static PropertyBuilder<TProperty> HasText<TEntity, TProperty>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, TProperty>> propertyExpression,
        int length
    )
        where TEntity : class => builder.Property(propertyExpression).IsUnicode(true).HasMaxLength(length);
}
