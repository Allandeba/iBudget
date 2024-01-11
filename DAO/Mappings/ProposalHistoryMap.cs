using iBudget.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iBudget.DAO.Mappings;

public class ProposalHistoryMap : IEntityTypeConfiguration<ProposalHistoryModel>
{
    public void Configure(EntityTypeBuilder<ProposalHistoryModel> builder)
    {
        _ = builder.OwnsOne(
            ph => ph.ProposalContentJSON,
            ownedNavigationBuilder =>
            {
                _ = ownedNavigationBuilder
                    .Property(pc => pc.ProposalContentItems)
                    .HasConversion(
                        v => Newtonsoft.Json.JsonConvert.SerializeObject(v),
                        v =>
                            Newtonsoft.Json.JsonConvert.DeserializeObject<
                                List<ProposalContentItems>
                            >(v),
                        new ValueComparer<List<ProposalContentItems>>(
                            (c1, c2) =>
                                Newtonsoft.Json.JsonConvert.SerializeObject(c1)
                                == Newtonsoft.Json.JsonConvert.SerializeObject(c2),
                            c =>
                                c == null
                                    ? 0
                                    : Newtonsoft.Json.JsonConvert.SerializeObject(c).GetHashCode(),
                            c =>
                                Newtonsoft.Json.JsonConvert.DeserializeObject<
                                    List<ProposalContentItems>
                                >(Newtonsoft.Json.JsonConvert.SerializeObject(c))
                        )
                    );
            }
        );
    }
}
