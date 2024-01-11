using iBudget.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iBudget.DAO.Mappings;

public class CompanyMap : IEntityTypeConfiguration<CompanyModel>
{
    public void Configure(EntityTypeBuilder<CompanyModel> builder) { }
}
