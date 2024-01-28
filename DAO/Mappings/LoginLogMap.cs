using iBudget.Framework;
using iBudget.DAO.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iBudget.DAO.Mappings;

public class LoginLogMap : IEntityTypeConfiguration<LoginLogModel>
{
    public void Configure(EntityTypeBuilder<LoginLogModel> builder)
    {
        _ = builder.ToTable("LoginLogs");

        _ = builder.HasKey(ll => ll.LoginLogId);

        _ = builder.Property(ll => ll.Username).HasColumnName("Username").IsRequired();

        _ = builder.Property(ll => ll.Password).HasColumnName("Password").IsRequired();

        _ = builder.Property(ll => ll.RemoteIpAddress).HasColumnName("RemoteIpAddress");

        _ = builder
            .Property(ll => ll.DateTime)
            .HasColumnName("DateTime")
            .HasColumnType("timestamp")
            .IsRequired();

        _ = builder.Property(ll => ll.Status).HasColumnName("Status").IsRequired();

        // todo: fazer um extension class para enums <T>
        List<string> enumValues = Enum.GetValues(typeof(LoginLogStatus))
            .Cast<LoginLogStatus>()
            .Select(e => $"{(int)e}-{e}")
            .ToList();

        string comment = string.Join(",", enumValues);

        _ = builder.Property(l => l.Status).HasComment(comment);
    }
}
