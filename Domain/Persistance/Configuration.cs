using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;

public class CommonEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : Common
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
               .HasColumnType("uuid")
               .HasDefaultValueSql("gen_random_uuid()")
               .HasColumnName("id");

        // Common columns
        builder.Property(x => x.CreatedBy)
               .HasColumnName("created_by")
               .HasMaxLength(100);

        builder.Property(x => x.CreatedDate)
               .HasColumnName("created_date")
               .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

        builder.Property(x => x.ModifiedBy)
               .HasColumnName("modified_by")
               .HasMaxLength(100);

        builder.Property(x => x.ModifiedDate)
               .HasColumnName("modified_date");

        builder.Property(x => x.IsActive)
               .HasDefaultValue(true)
               .HasColumnName("is_active");

        // Global query filter for IsActive
        builder.HasQueryFilter(e => e.IsActive);

        // Convert table name and properties to snake_case
        builder.ToTable(ToSnakeCase(typeof(TEntity).Name));

        foreach (var property in typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            // Only map simple types, not navigation properties
            if (property.Name != nameof(Common.Id) && 
                (property.PropertyType.IsValueType || property.PropertyType == typeof(string)))
            {
                builder.Property(property.Name).HasColumnName(ToSnakeCase(property.Name));
            }
        }
    }

    private static string ToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        var builder = new System.Text.StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            var c = input[i];
            if (char.IsUpper(c))
            {
                if (i > 0) builder.Append('_');
                builder.Append(char.ToLower(c));
            }
            else
            {
                builder.Append(c);
            }
        }
        return builder.ToString();
    }
}
