using getQuote.Models;
using getQuote.Repository;

namespace getQuote.Business
{
    public class CatalogBusiness
    {
        private readonly CatalogRepository _repository;

        public CatalogBusiness(CatalogRepository catalogRepository)
        {
            _repository = catalogRepository;
        }

        public async Task<IEnumerable<ItemModel>> GetItems()
        {
            return await _repository.GetItems();
        }
    }
}
