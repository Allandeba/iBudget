using X.PagedList;

namespace iBudget.DAO.Entities;

public class PaginationItem
{
    public IPagedList Model { get; set; }
    public string Action { get; set; }
    public string SearchValue { get; set; }
}