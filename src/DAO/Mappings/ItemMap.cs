using iBudget.DAO.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iBudget.DAO.Mappings;

public class ItemMap : IEntityTypeConfiguration<ItemModel>
{
    public void Configure(EntityTypeBuilder<ItemModel> builder)
    {
        _ = builder.ToTable("Items");

        _ = builder.HasKey(i => i.ItemId);

        _ = builder
            .Property(i => i.ItemName)
            .HasColumnName("ItemName")
            .IsRequired()
            .HasMaxLength(50);

        _ = builder
            .Property(i => i.Value)
            .HasColumnName("Value")
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        _ = builder
            .Property(i => i.Description)
            .HasColumnName("Description")
            .IsRequired()
            .HasMaxLength(250);

        _ = builder.HasIndex(p => p.ItemName).IsUnique();
    }
}