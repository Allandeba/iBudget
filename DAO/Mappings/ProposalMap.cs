using iBudget.DAO.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iBudget.DAO.Mappings;

public class ProposalMap : IEntityTypeConfiguration<ProposalModel>
{
    public void Configure(EntityTypeBuilder<ProposalModel> builder)
    {
        _ = builder
            .Property(p => p.ModificationDate)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAddOrUpdate();

        _ = builder.HasIndex(p => p.GUID).IsUnique();
    }
}
