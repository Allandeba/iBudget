using iBudget.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iBudget.DAO.Mappings;

public class DocumentMap : IEntityTypeConfiguration<DocumentModel>
{
    public void Configure(EntityTypeBuilder<DocumentModel> builder)
    {
        _ = builder
            .HasOne(d => d.Person)
            .WithOne(p => p.Document)
            .HasForeignKey<DocumentModel>(d => d.PersonId);
    }
}