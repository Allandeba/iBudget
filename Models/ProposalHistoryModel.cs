﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iBudget.Models;

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
        ProposalContentItems = new();
    }
    
    public List<ProposalContentItems> ProposalContentItems { get; set; }

    public List<int> GetItemIds()
    {
        List<int> itemIds = new();
        foreach (ProposalContentItems proposalContentItem in ProposalContentItems)
        {
            itemIds.Add(int.Parse(proposalContentItem.ItemId));
        }

        return itemIds;
    }
}

public class ProposalHistoryModel
{
    public ProposalHistoryModel()
    {
        ModificationDate = DateTime.MinValue;
        Discount = 0.0m;
        ProposalContentJSON = new();
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
    public ProposalModel Proposal { get; set; }

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    public PersonModel Person { get; set; }

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [Column(TypeName = "jsonb")]
    public ProposalContentJSON ProposalContentJSON { get; set; }
}
