using iBudget.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iBudget.DAO.Mappings;

public class ItemImageMap : IEntityTypeConfiguration<ItemImageModel>
{
    public void Configure(EntityTypeBuilder<ItemImageModel> builder)
    {

    }
}