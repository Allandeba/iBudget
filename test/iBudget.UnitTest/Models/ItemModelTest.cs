using iBudget.DAO.Entities;
using iBudget.Framework;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;

namespace iBudget.UnitTest.Models;

public class ItemModelTest
{
    private FormFile GetFormFile(string nomeItem, string nomeArquivo)
    {
        Image image = new Image<Rgba32>(100, 100);
        MemoryStream memoryStream = new();
        image.Save(memoryStream, new PngEncoder());
        var formFile = new FormFile(
            memoryStream,
            0,
            memoryStream.Length,
            nomeItem,
            nomeArquivo
        )
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/png"
        };

        return formFile;
    }

    private void FetchImageList(int numImages, List<IFormFile> imageFiles)
    {
        for (var i = 0; i < numImages; i++)
        {
            var nomeItem = "Item_" + i;
            var nomeArquivo = "File_" + i;
            imageFiles.Add(GetFormFile(nomeItem, nomeArquivo));
        }
    }

    [Fact]
    public void ShouldSetLastAddedImageAsMainImage()
    {
        ItemModel item = new();
        FetchImageList(3, item.ImageFiles);

        item.SetItemImageList();
        item.SetDefaultImage(item.ImageFiles.Last().FileName);

        var itemImage = item.ItemImageList.Find(i => i.Main);
        Assert.NotNull(itemImage);

        var isLastImageDefault = itemImage.FileName == item.ImageFiles.Last().FileName;
        Assert.True(isLastImageDefault);
    }

    [Fact]
    public void ShouldNotHaveMainImage()
    {
        ItemModel item = new();
        FetchImageList(3, item.ImageFiles);

        item.SetItemImageList();
        item.SetDefaultImage(SelectDefault.Nenhum.ToString());

        var itemImage = item.ItemImageList.Find(i => i.Main);
        Assert.Null(itemImage);
    }

    [Fact]
    public void ShouldReturnMainImage()
    {
        ItemModel item = new();
        FetchImageList(3, item.ImageFiles);

        item.SetItemImageList();
        item.SetDefaultImage(item.ImageFiles.First().FileName);

        var itemImage = item.GetMainImage();
        Assert.Equal(item.ImageFiles.First().FileName, itemImage.FileName);
    }
}