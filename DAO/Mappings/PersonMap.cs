using iBudget.DAO.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iBudget.DAO.Mappings;

public class PersonMap : IEntityTypeConfiguration<PersonModel>
{
    public void Configure(EntityTypeBuilder<PersonModel> builder)
    {
        _ = builder
            .HasOne(PersonModel => PersonModel.Contact)
            .WithOne(c => c.Person)
            .HasForeignKey<ContactModel>(c => c.PersonId);

        _ = builder.ToTable("Persons");

        _ = builder.HasKey(p => p.PersonId);

        _ = builder
            .Property(p => p.FirstName)
            .HasColumnName("FirstName")
            .IsRequired()
            .HasMaxLength(50);

        _ = builder
            .Property(p => p.LastName)
            .HasColumnName("LastName")
            .IsRequired()
            .HasMaxLength(100);

        _ = builder
            .Property(p => p.CreationDate)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        _ = builder
            .HasOne(p => p.Document)
            .WithOne()
            .HasForeignKey<PersonModel>(p => p.DocumentId)
            .IsRequired();

        _ = builder
            .HasOne(p => p.Contact)
            .WithOne()
            .HasForeignKey<PersonModel>(p => p.ContactId)
            .IsRequired();
    }
}
