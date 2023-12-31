using iBudget.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iBudget.DAO.Mappings;

public class PersonMap : IEntityTypeConfiguration<PersonModel>
{
    public void Configure(EntityTypeBuilder<PersonModel> builder)
    {
        _ = builder
            .Property(p => p.CreationDate)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        _ = builder
            .HasOne(PersonModel => PersonModel.Document)
            .WithOne(d => d.Person)
            .HasForeignKey<DocumentModel>(d => d.PersonId);

        _ = builder
            .HasOne(PersonModel => PersonModel.Contact)
            .WithOne(c => c.Person)
            .HasForeignKey<ContactModel>(c => c.PersonId);
    }
}