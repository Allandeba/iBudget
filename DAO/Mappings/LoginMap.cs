using iBudget.DAO.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iBudget.DAO.Mappings;

public class LoginMap : IEntityTypeConfiguration<LoginModel>
{
    public void Configure(EntityTypeBuilder<LoginModel> builder)
    {
        _ = builder.ToTable("Logins");

        _ = builder.HasKey(l => l.LoginId);

        _ = builder
            .Property(l => l.Username)
            .HasColumnName("Username")
            .IsRequired()
            .HasMaxLength(25);

        _ = builder
            .Property(l => l.Password)
            .HasColumnName("Password")
            .IsRequired()
            .HasMaxLength(50);

        _ = builder.HasIndex(p => p.Username).IsUnique();
    }
}
