using Bogus;
using System.Diagnostics;

namespace iBudget.Models.FakeModels.Helpers;

public static class FakerHelper
{
    private const int MaxAttempts = 10;

    public static async Task<byte[]> GetRandomImage()
    {
        using var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
        var faker = new Faker();
        for (var attempt = 1; ; attempt++)
        {
            var companyImageUrl = ((attempt - 1) % 3) switch
            {
                0 => faker.Image.PicsumUrl(),
                1 => faker.Image.LoremFlickrUrl(width: 640, height: 480),
                _ => faker.Image.PlaceholderUrl(width: 640, height: 480, format: "png"),
            };
            var stopwatch = Stopwatch.StartNew();
            Console.WriteLine(
                $"[FakerHelper] Download {attempt}/{MaxAttempts}: {companyImageUrl} (timeout: {httpClient.Timeout.TotalSeconds}s)");
            try
            {
                var image = await httpClient.GetByteArrayAsync(companyImageUrl);
                Console.WriteLine(
                    $"[FakerHelper] Download {attempt}/{MaxAttempts} concluído: {companyImageUrl}; {stopwatch.ElapsedMilliseconds}ms; {image.Length} bytes.");
                return image;
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine(
                    $"[FakerHelper] Download {attempt}/{MaxAttempts} falhou: {companyImageUrl}; {stopwatch.ElapsedMilliseconds}ms. {exception}");
                if (attempt == MaxAttempts)
                    throw;
            }
        }
    }
}
