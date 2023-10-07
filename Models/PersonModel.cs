using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace getQuote.Models;

public class PersonModel
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
    public int PersonId { get; set; }

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [Display(Name = "First name")]
    [MaxLength(50, ErrorMessage = Messages.MaxLengthValidation)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [Display(Name = "Last name")]
    [MaxLength(100, ErrorMessage = Messages.MaxLengthValidation)]
    public string LastName { get; set; } = string.Empty;

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constants.DateFormat)]
    [DataType(DataType.DateTime, ErrorMessage = Messages.InvalidFormatValidation)]
    public DateTime CreationDate { get; set; } = DateTime.MinValue;

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    public virtual required DocumentModel Document { get; set; }

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    public virtual required ContactModel Contact { get; set; }

    public virtual List<ProposalModel>? Proposal { get; set; }

    public string PersonName => FirstName + ' ' + LastName;
}
