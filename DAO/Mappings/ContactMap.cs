using iBudget.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iBudget.DAO.Mappings;

public class ContactMap : IEntityTypeConfiguration<ContactModel>
{
    public void Configure(EntityTypeBuilder<ContactModel> builder)
    {
        _ = builder
            .HasOne(d => d.Person)
            .WithOne(p => p.Contact)
            .HasForeignKey<ContactModel>(d => d.PersonId);
    }
}