using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iBudget.Models;

public class ProposalModel
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
    public int ProposalId { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constants.DateFormat)]
    public DateTime ModificationDate { get; set; } = DateTime.MinValue;

    [DataType(DataType.Currency, ErrorMessage = Messages.InvalidFormatValidation)]
    [Range(0, int.MaxValue, ErrorMessage = Messages.MinValueValidation)]
    [Precision(18, 2)]
    [DisplayFormat(DataFormatString = "{0:#.##}")]
    public double Discount { get; set; } = 0;

    public Guid GUID { get; set; } = Guid.NewGuid();

    [Display(Name = "Person")]
    public int PersonId { get; set; } = 0;
    public virtual required PersonModel? Person { get; set; }

    public virtual required List<ProposalContentModel> ProposalContent { get; set; }
    public virtual List<ProposalHistoryModel>? ProposalHistory { get; set; }

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
