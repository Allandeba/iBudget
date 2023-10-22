using iBudget.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iBudget.Models;

public class ECompanyBusinessException : Exception
{
    public ECompanyBusinessException(string message)
        : base(message) { }
}

public class CompanyModel
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
    public int CompanyId { get; set; }

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [Display(Name = "Nome da empresa")]
    [MaxLength(50, ErrorMessage = Messages.MaxLengthValidation)]
    public string CompanyName { get; set; } = string.Empty;

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [MaxLength(20, ErrorMessage = Messages.MaxLengthValidation)]
    public string CNPJ { get; set; } = string.Empty;

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [Display(Name = "Endereço")]
    [MaxLength(150, ErrorMessage = Messages.MaxLengthValidation)]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [Display(Name = "E-mail")]
    [MaxLength(50, ErrorMessage = Messages.MaxLengthValidation)]
    [DataType(DataType.EmailAddress, ErrorMessage = Messages.InvalidFormatValidation)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [Display(Name = "Celular")]
    [MaxLength(25, ErrorMessage = Messages.MaxLengthValidation)]
    [DataType(DataType.PhoneNumber, ErrorMessage = Messages.InvalidFormatValidation)]
    public string Phone { get; set; } = string.Empty;

    public byte[]? ImageFile { get; set; } = null!;

    [NotMapped]
    [Display(Name = "Logomarca")]
    [DataType(DataType.Upload, ErrorMessage = Messages.InvalidFormatValidation)]
    public IFormFile? FormImageFile { get; set; }

    public void SetNewImage()
    {
        if (FormImageFile != null)
        {
            ImageFile = ImageUtils
                .ResizeImage(
                    FormImageFile,
                    Constants.MaxImageLogoWidth,
                    Constants.MaxImageLogoHeight
                )
                .ToArray();
        }
    }
}
