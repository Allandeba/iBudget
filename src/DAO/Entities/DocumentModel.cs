using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared;
using Shared.Extensions;

namespace iBudget.DAO.Entities;

public class DocumentModel
{
    private string _document = string.Empty;

    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    [Key]
    public int DocumentId { get; set; }

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [Display(Name = "Tipo de documento")]
    public DocumentTypes DocumentType { get; set; }

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [Display(Name = "Documento")]
    [MaxLength(50, ErrorMessage = Messages.MaxLengthValidation)]
    public string Document
    {
        get => _document;
        set => _document = value.OnlyNumbers();
    }

    public int PersonId { get; set; }
    public PersonModel Person { get; set; }
}