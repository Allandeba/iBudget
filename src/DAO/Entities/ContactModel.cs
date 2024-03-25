using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iBudget.Framework;
using Shared;

namespace iBudget.DAO.Entities;

public class ContactModel
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    [Key]
    public int ContactId { get; set; }

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [Display(Name = "E-mail")]
    [MaxLength(50, ErrorMessage = Messages.MaxLengthValidation)]
    [DataType(DataType.EmailAddress, ErrorMessage = Messages.InvalidFormatValidation)]
    public string Email { get; set; }

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [Display(Name = "Celular")]
    [MaxLength(25, ErrorMessage = Messages.MaxLengthValidation)]
    [DataType(DataType.PhoneNumber, ErrorMessage = Messages.InvalidFormatValidation)]
    public string Phone { get; set; }

    public int PersonId { get; set; }
    public PersonModel Person { get; set; }
}