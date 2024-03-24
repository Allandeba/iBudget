using iBudget.DAO.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iBudget.DAO.Mappings;

public class ItemImageMap : IEntityTypeConfiguration<ItemImageModel>
{
    public void Configure(EntityTypeBuilder<ItemImageModel> builder)
    {
        _ = builder.ToTable("ItemImages");

        _ = builder.HasKey(iim => iim.ItemImageId);

        _ = builder.Property(iim => iim.Main).HasColumnName("Main").IsRequired();

        _ = builder
            .Property(iim => iim.FileName)
            .HasColumnName("FileName")
            .IsRequired()
            .HasMaxLength(100);

        _ = builder.Property(iim => iim.ImageFile).HasColumnName("ImageFile").IsRequired();

        _ = builder
            .HasOne(iim => iim.Item)
            .WithMany(im => im.ItemImageList)
            .HasForeignKey(iim => iim.ItemId)
            .IsRequired();
    }
}