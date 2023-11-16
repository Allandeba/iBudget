using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iBudget.Models;

public class ProposalModel
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
    public int ProposalId { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constants.DateFormat)]
    [Display(Name = "Data da modificação")]
    public DateTime ModificationDate { get; set; } = DateTime.MinValue;

    [DataType(DataType.Currency, ErrorMessage = Messages.InvalidFormatValidation)]
    [Display(Name = "Desconto")]
    [Range(0, int.MaxValue, ErrorMessage = Messages.MinValueValidation)]
    [Precision(18, 2)]
    [DisplayFormat(DataFormatString = "{0:#.##}")]
    public double Discount { get; set; } = 0;

    public Guid GUID { get; set; } = Guid.NewGuid();

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = Messages.EmptyTextValidation)]
    [Display(Name = "Pessoa")]
    public int PersonId { get; set; }
    public virtual required PersonModel Person { get; set; }
    
    [Required(ErrorMessage = "Você deve adicionar um ou mais produtos para criar um orçamento")]
    public virtual required List<ProposalContentModel> ProposalContent { get; set; }
    public virtual List<ProposalHistoryModel> ProposalHistory { get; set; }

    public List<int> GetIdProposalContentList()
    {
        List<int> proposalContentIdList = new();
        foreach (ProposalContentModel ProposalContent in ProposalContent)
        {
            proposalContentIdList.Add(ProposalContent.ProposalContentId);
        }

        return proposalContentIdList;
    }
}
