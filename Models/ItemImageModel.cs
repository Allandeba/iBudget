using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace getQuote.Models;

public class ItemImageModel
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
    public int ItemImageId { get; set; }

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    public bool Main { get; set; } = false;

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [MaxLength(100, ErrorMessage = Messages.MaxLengthValidation)]
    public string FileName { get; set; } = string.Empty;

    [Display(Name = "Upload Image")]
    public byte[] ImageFile { get; set; } = Array.Empty<byte>();

    public int ItemId { get; set; }
    public virtual required ItemModel Item { get; set; }
}
