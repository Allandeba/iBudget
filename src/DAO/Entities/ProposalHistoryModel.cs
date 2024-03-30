using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace iBudget.DAO.Entities;

[Owned]
public class ProposalContentItems
{
    public required string ItemId { get; set; }
    public required string Quantity { get; set; }
}

[Owned]
public class ProposalContentJSON
{
    public ProposalContentJSON()
    {
        ProposalContentItems = new List<ProposalContentItems>();
    }

    public List<ProposalContentItems> ProposalContentItems { get; set; }

    public List<int> GetItemIds()
    {
        List<int> itemIds = new();
        foreach (var proposalContentItem in ProposalContentItems) itemIds.Add(int.Parse(proposalContentItem.ItemId));

        return itemIds;
    }
}

public class ProposalHistoryModel
{
    public ProposalHistoryModel()
    {
        ModificationDate = DateTime.MinValue;
        Discount = 0.0m;
        ProposalContentJSON = new ProposalContentJSON();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProposalHistoryId { get; set; }

    public DateTime ModificationDate { get; set; }

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [Display(Name = "Desconto")]
    [DataType(DataType.Currency, ErrorMessage = Messages.InvalidFormatValidation)]
    [Precision(18, 2)]
    public decimal Discount { get; set; }

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    public int ProposalId { get; set; }

    public ProposalModel Proposal { get; set; }

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    public int PersonId { get; set; }

    public PersonModel Person { get; set; }

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [Column(TypeName = "jsonb")]
    public ProposalContentJSON ProposalContentJSON { get; set; }
}