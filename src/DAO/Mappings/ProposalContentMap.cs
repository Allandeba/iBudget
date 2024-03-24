using iBudget.DAO.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iBudget.DAO.Mappings;

public class ProposalContentMap : IEntityTypeConfiguration<ProposalContentModel>
{
    public void Configure(EntityTypeBuilder<ProposalContentModel> builder)
    {
        _ = builder.ToTable("ProposalContents");

        _ = builder.HasKey(pc => pc.ProposalContentId);

        _ = builder.Property(pc => pc.Quantity).HasColumnName("Quantity").IsRequired();

        _ = builder.Property(pc => pc.ProposalId).HasColumnName("ProposalId").IsRequired();

        _ = builder.Property(pc => pc.ItemId).HasColumnName("ItemId").IsRequired();

        _ = builder
            .HasOne(pc => pc.Proposal)
            .WithMany(p => p.ProposalContent)
            .HasForeignKey(pc => pc.ProposalId)
            .IsRequired();

        _ = builder
            .HasOne(pc => pc.Item)
            .WithMany()
            .HasForeignKey(pc => pc.ItemId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}