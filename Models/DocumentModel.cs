using getQuote.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace getQuote.Models;

public class DocumentModel
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
    public int DocumentId { get; set; }

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [Display(Name = "Document type")]
    public DocumentTypes DocumentType { get; set; }

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [MaxLength(50, ErrorMessage = Messages.MaxLengthValidation)]
    public string Document { get; set; } = string.Empty;

    public int PersonId { get; set; }
    public virtual PersonModel? Person { get; set; }
}
