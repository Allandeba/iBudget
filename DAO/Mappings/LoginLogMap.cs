using iBudget.Framework;
using iBudget.DAO.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iBudget.DAO.Mappings;

public class LoginLogMap : IEntityTypeConfiguration<LoginLogModel>
{
    public void Configure(EntityTypeBuilder<LoginLogModel> builder)
    {
        List<string> enumValues = Enum.GetValues(typeof(LoginLogStatus))
            .Cast<LoginLogStatus>()
            .Select(e => $"{(int)e}-{e}")
            .ToList();

        string comment = string.Join(",", enumValues);

        _ = builder.Property(l => l.Status).HasComment(comment);
    }
}
