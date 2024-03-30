using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared;

namespace iBudget.DAO.Entities;

public class ProposalContentModel
{
    public ProposalContentModel()
    {
        Quantity = 1;
    }

    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    [Key]
    public int ProposalContentId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = Messages.MinValueValidation)]
    [Display(Name = "Quantidade")]
    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    public int Quantity { get; set; }

    [Required] public int ProposalId { get; set; }

    public ProposalModel Proposal { get; set; }

    [Required] public int ItemId { get; set; }

    public ItemModel Item { get; set; }
}