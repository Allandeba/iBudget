using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iBudget.Models;

public class LoginModel
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
    public int LoginId { get; set; }

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [Display(Name = "Usuário")]
    [MaxLength(25, ErrorMessage = Messages.MaxLengthValidation)]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [DataType(DataType.Password)]
    [Display(Name = "Senha")]
    [MaxLength(50, ErrorMessage = Messages.MaxLengthValidation)]
    [RegularExpression(
        @Constants.PasswordValidationRegex,
        ErrorMessage = Messages.PasswordValidation
    )]
    public string Password { get; set; } = string.Empty;

    [NotMapped]
    [Display(Name = "Lembrar-me")]
    public bool Remember { get; set; } = false;
}
