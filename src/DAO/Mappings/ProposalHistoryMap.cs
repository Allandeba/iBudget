using iBudget.DAO.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace iBudget.DAO.Mappings;

public class ProposalHistoryMap : IEntityTypeConfiguration<ProposalHistoryModel>
{
    public void Configure(EntityTypeBuilder<ProposalHistoryModel> builder)
    {
        _ = builder.ToTable("ProposalHistories");

        _ = builder.HasKey(ph => ph.ProposalHistoryId);

        _ = builder
            .Property(ph => ph.ModificationDate)
            .HasColumnName("ModificationDate")
            .HasColumnType("timestamp");

        _ = builder
            .Property(ph => ph.Discount)
            .HasColumnName("Discount")
            .HasColumnType("decimal(18,2)");

        _ = builder.Property(ph => ph.ProposalId).HasColumnName("ProposalId");

        _ = builder.Property(ph => ph.PersonId).HasColumnName("PersonId");

        _ = builder
            .HasOne(ph => ph.Proposal)
            .WithMany(p => p.ProposalHistory)
            .HasForeignKey(ph => ph.ProposalId)
            .IsRequired();

        _ = builder
            .HasOne(ph => ph.Person)
            .WithMany()
            .HasForeignKey(ph => ph.PersonId)
            .OnDelete(DeleteBehavior.Restrict);

        _ = builder.OwnsOne(
            ph => ph.ProposalContentJSON,
            ownedNavigationBuilder =>
            {
                _ = ownedNavigationBuilder
                    .Property(pc => pc.ProposalContentItems)
                    .HasConversion(
                        v => JsonConvert.SerializeObject(v),
                        v =>
                            JsonConvert.DeserializeObject<
                                List<ProposalContentItems>
                            >(v),
                        new ValueComparer<List<ProposalContentItems>>(
                            (c1, c2) =>
                                JsonConvert.SerializeObject(c1)
                                == JsonConvert.SerializeObject(c2),
                            c =>
                                c == null
                                    ? 0
                                    : JsonConvert.SerializeObject(c).GetHashCode(),
                            c =>
                                JsonConvert.DeserializeObject<
                                    List<ProposalContentItems>
                                >(JsonConvert.SerializeObject(c))
                        )
                    );
            }
        );
    }
}