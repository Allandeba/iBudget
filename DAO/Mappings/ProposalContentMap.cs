using iBudget.DAO.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iBudget.DAO.Mappings;

public class ProposalContentMap : IEntityTypeConfiguration<ProposalContentModel>
{
    public void Configure(EntityTypeBuilder<ProposalContentModel> builder) { }
}
