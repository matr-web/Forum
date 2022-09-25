namespace Forum.WebAPI.Pagination;

public class PagedResult<T>
{
    public PagedResult(List<T> items, int totalCount, int PageSize, int PageNumber)
    {
        Items = items;
        TotalItemsCount = totalCount;
        ItemsFrom = PageSize * (PageNumber - 1) + 1;
        ItemsTo = ItemsFrom + (PageSize - 1);
        TotalPages = (int)Math.Ceiling(TotalItemsCount /(double) PageSize);
    }

    public List<T> Items { get; set; }

    public int TotalPages { get; set; }

    public int ItemsFrom { get; set; }

    public int ItemsTo { get; set; }

    public int TotalItemsCount { get; set; }
}
