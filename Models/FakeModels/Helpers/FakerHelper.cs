using System.Net;
using Bogus;

namespace iBudget.Models.FakeModels.Helpers
{
    public static class FakerHelper
    {
        private static int retries;
        private static readonly int maxRetries = 10;

        public static async Task<byte[]> GetRandomImage()
        {
            try
            {
                var companyImageUrl = new Faker().Image.PicsumUrl();

                using (HttpClient httpClient = new())
                    return await httpClient.GetByteArrayAsync(companyImageUrl);
            }
            catch (Exception)
            {
                // Pode retornar uma URL inválida, e quando converter retorna 404
                retries++;
                if (retries < maxRetries)
                    return await GetRandomImage();
                throw;
            }
        }
    }
}
