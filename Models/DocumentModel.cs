using iBudget.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iBudget.Models;

public class DocumentModel
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
    public int DocumentId { get; set; }

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [Display(Name = "Tipo de documento")]
    public DocumentTypes DocumentType { get; set; }

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [Display(Name = "Documento")]
    [MaxLength(50, ErrorMessage = Messages.MaxLengthValidation)]
    public string Document { get; set; }

    [ForeignKey("Person")]
    public int PersonId { get; set; }
    public PersonModel Person { get; set; }
}
