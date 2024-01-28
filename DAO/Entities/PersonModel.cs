using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iBudget.DAO.Entities;

public class PersonModel
{
    public PersonModel()
    {
        CreationDate = DateTime.MinValue;
    }

    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
    public int PersonId { get; set; }

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [Display(Name = "Primeiro nome")]
    [MaxLength(50, ErrorMessage = Messages.MaxLengthValidation)]
    public string FirstName { get; set; }

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [Display(Name = "Segundo nome")]
    [MaxLength(100, ErrorMessage = Messages.MaxLengthValidation)]
    public string LastName { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constants.DateFormat)]
    [Display(Name = "Data da criação")]
    [DataType(DataType.DateTime, ErrorMessage = Messages.InvalidFormatValidation)]
    public DateTime CreationDate { get; set; }

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    public int DocumentId { get; set; }
    public DocumentModel Document { get; set; }

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    public int ContactId { get; set; }
    public ContactModel Contact { get; set; }

    public List<ProposalModel> Proposal { get; set; }

    public string PersonName => FirstName + ' ' + LastName;
}
