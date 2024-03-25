using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iBudget.Framework;
using Shared;

namespace iBudget.DAO.Entities;

public class ItemImageModel
{
    public ItemImageModel()
    {
        ImageFile = Array.Empty<byte>();
    }

    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    [Key]
    public int ItemImageId { get; set; }

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [Display(Name = "Principal")]
    public bool Main { get; set; }

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [Display(Name = "Nome do arquivo")]
    [MaxLength(100, ErrorMessage = Messages.MaxLengthValidation)]
    public string FileName { get; set; }

    [Display(Name = "Imagens")] public byte[] ImageFile { get; set; }

    [Required] public int ItemId { get; set; }

    public ItemModel Item { get; set; }
}