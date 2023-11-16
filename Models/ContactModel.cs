﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iBudget.Models;

public class ContactModel
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
    public int ContactId { get; set; }

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

    public int PersonId { get; set; }
    public virtual PersonModel Person { get; set; }
}
