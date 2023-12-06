using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iBudget.Models;

public class ProposalModel
{
    public ProposalModel()
    {
        ModificationDate = DateTime.MinValue;
        Discount = 0.0m;
        GUID = Guid.NewGuid();
    }

    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
    public int ProposalId { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constants.DateFormat)]
    [Display(Name = "Data da modificação")]
    public DateTime ModificationDate { get; set; }

    [DataType(DataType.Currency, ErrorMessage = Messages.InvalidFormatValidation)]
    [Display(Name = "Desconto")]
    [Range(0, int.MaxValue, ErrorMessage = Messages.MinValueValidation)]
    [Precision(18, 2)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
    public decimal Discount { get; set; }

    public Guid GUID { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = Messages.EmptyTextValidation)]
    [Display(Name = "Pessoa")]
    public int PersonId { get; set; }
    public PersonModel Person { get; set; }

    [Required(ErrorMessage = "Você deve adicionar um ou mais produtos para criar um orçamento")]
    public List<ProposalContentModel> ProposalContent { get; set; }
    [ForeignKey("ProposalHistoryId")]
    public List<ProposalHistoryModel> ProposalHistory { get; set; }

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
