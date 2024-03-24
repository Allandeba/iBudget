using iBudget.DAO.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iBudget.DAO.Mappings;

public class ContactMap : IEntityTypeConfiguration<ContactModel>
{
    public void Configure(EntityTypeBuilder<ContactModel> builder)
    {
        _ = builder.ToTable("Contacts");

        _ = builder.HasKey(c => c.ContactId);

        _ = builder
            .Property(c => c.Email)
            .HasColumnName("Email")
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnType("VARCHAR(50)");

        _ = builder
            .Property(c => c.Phone)
            .HasColumnName("Phone")
            .IsRequired()
            .HasMaxLength(25)
            .HasColumnType("VARCHAR(25)");

        _ = builder
            .HasOne(c => c.Person)
            .WithOne(p => p.Contact)
            .HasForeignKey<ContactModel>(c => c.PersonId)
            .IsRequired();

        _ = builder.HasIndex(p => p.Phone).IsUnique();
    }
}