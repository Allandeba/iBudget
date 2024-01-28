using iBudget.DAO.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iBudget.DAO.Mappings;

public class DocumentMap : IEntityTypeConfiguration<DocumentModel>
{
    public void Configure(EntityTypeBuilder<DocumentModel> builder)
    {
        _ = builder.ToTable("Documents");

        _ = builder.HasKey(d => d.DocumentId);

        _ = builder.Property(d => d.DocumentType).HasColumnName("DocumentType").IsRequired();

        _ = builder
            .Property(d => d.Document)
            .HasColumnName("Document")
            .IsRequired()
            .HasMaxLength(50);

        _ = builder
            .HasOne(d => d.Person)
            .WithOne(p => p.Document)
            .HasForeignKey<DocumentModel>(d => d.PersonId)
            .IsRequired();

        _ = builder.HasIndex(p => p.Document).IsUnique();
    }
}
