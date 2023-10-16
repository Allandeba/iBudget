using iBudget.Framework;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iBudget.Models;

public class ItemModel
{
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
    public int ItemId { get; set; }

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [Display(Name = "Item")]
    [MaxLength(50, ErrorMessage = Messages.MaxLengthValidation)]
    public string ItemName { get; set; } = string.Empty;

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [Display(Name = "Valor")]
    [Range(0, int.MaxValue, ErrorMessage = Messages.MinValueValidation)]
    [DataType(DataType.Currency, ErrorMessage = Messages.InvalidFormatValidation)]
    [Precision(18, 2)]
    [DisplayFormat(DataFormatString = "{0:#.##}")]
    public double Value { get; set; } = 0;

    [Required(ErrorMessage = Messages.EmptyTextValidation)]
    [Display(Name = "Descrição")]
    [MaxLength(250, ErrorMessage = Messages.MaxLengthValidation)]
    public string Description { get; set; } = string.Empty;

    [NotMapped]
    [Display(Name = "Imagens")]
    [DataType(DataType.Upload, ErrorMessage = Messages.InvalidFormatValidation)]
    public List<IFormFile> ImageFiles { get; set; } = new();

    [NotMapped]
    [Display(Name = "Imagem principal")]
    public string DefaultImage { get; set; } = string.Empty;

    [NotMapped]
    public List<int>? IdImagesToDelete { get; set; } = new();

    public void SetItemImageList()
    {
        if (ImageFiles != null && ImageFiles.Count > 0)
        {
            ItemImageList = new List<ItemImageModel>();
            foreach (IFormFile image in ImageFiles)
            {
                ItemImageModel itemImage =
                    new()
                    {
                        FileName = image.FileName,
                        ImageFile = ImageUtils
                            .ResizeImage(image, Constants.MaxImageWidth, Constants.MaxImageHeight)
                            .ToArray(),
                        Item = new ItemModel()
                    };

                ItemImageList.Add(itemImage);
            }
        }
    }

    public void SetDefaultImage(string defaultImageFileName)
    {
        if (ItemImageList != null && ItemImageList.Count > 0)
        {
            foreach (ItemImageModel image in ItemImageList)
            {
                image.Main =
                    defaultImageFileName != SelectDefault.Nenhum.ToString()
                    && image.FileName == defaultImageFileName;
            }
        }
    }

    public virtual List<ItemImageModel>? ItemImageList { get; set; }
    public virtual List<ProposalContentModel>? ProposalContent { get; set; }

    public ItemImageModel GetMainImage()
    {
        ItemImageModel? itemImageModel = ItemImageList.FirstOrDefault(a => a.Main);
        return itemImageModel;
    }
}
