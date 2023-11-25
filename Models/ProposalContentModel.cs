using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iBudget.Models;

public class ProposalContentModel
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
    public int ProposalContentId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = Messages.MinValueValidation)]
    [Display(Name = "Quantidade")]
    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    public int Quantity { get; set; } = 1;

    public int ProposalId { get; set; }
    public virtual required ProposalModel Proposal { get; set; }

    public int ItemId { get; set; }
    public virtual required ItemModel Item { get; set; }
}
