using iBudget.DAO.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iBudget.DAO.Mappings;

public class ProposalMap : IEntityTypeConfiguration<ProposalModel>
{
    public void Configure(EntityTypeBuilder<ProposalModel> builder)
    {
        _ = builder.ToTable("Proposals");

        _ = builder.HasKey(p => p.ProposalId);

        _ = builder
            .Property(p => p.ModificationDate)
            .HasColumnName("ModificationDate")
            .HasColumnType("timestamp")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAddOrUpdate();

        _ = builder
            .Property(p => p.Discount)
            .HasColumnName("Discount")
            .HasColumnType("decimal(18,2)");

        _ = builder.Property(p => p.GUID).HasColumnName("GUID").HasColumnType("uuid");

        _ = builder.HasIndex(p => p.GUID).IsUnique();

        _ = builder
            .HasOne(p => p.Person)
            .WithMany()
            .HasForeignKey(p => p.PersonId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        _ = builder
            .HasMany(p => p.ProposalContent)
            .WithOne(pc => pc.Proposal)
            .HasForeignKey(pc => pc.ProposalId)
            .IsRequired();

        _ = builder
            .HasMany(p => p.ProposalHistory)
            .WithOne(ph => ph.Proposal)
            .HasForeignKey(ph => ph.ProposalId)
            .IsRequired();
    }
}