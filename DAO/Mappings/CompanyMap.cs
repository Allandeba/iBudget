using iBudget.DAO.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iBudget.DAO.Mappings;

public class CompanyMap : IEntityTypeConfiguration<CompanyModel>
{
    public void Configure(EntityTypeBuilder<CompanyModel> builder)
    {
        _ = builder.ToTable("Companies");

        _ = builder.HasKey(c => c.CompanyId);

        _ = builder
            .Property(c => c.CompanyName)
            .HasColumnName("CompanyName")
            .IsRequired()
            .HasMaxLength(50);

        _ = builder.Property(c => c.CNPJ).HasColumnName("CNPJ").IsRequired().HasMaxLength(20);

        _ = builder
            .Property(c => c.Address)
            .HasColumnName("Address")
            .IsRequired()
            .HasMaxLength(150);

        _ = builder
            .Property(c => c.Email)
            .HasColumnName("Email")
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnType("VARCHAR(50)");

        _ = builder
            .Property(c => c.Phone)
            .HasColumnName("Phone")
            .IsRequired()
            .HasMaxLength(25)
            .HasColumnType("VARCHAR(25)");

        _ = builder.Property(c => c.ImageFile).HasColumnName("ImageFile").HasColumnType("bytea");
    }
}
