namespace Forum.WebAPI.Dto_s;

public class Query
{
    public string SearchPhrase { get; set; } = null;

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 15;

    public string SortBy { get; set; } = "Topic";

    public SortOrder SortOrder { get; set; } = SortOrder.ASC;
}

public enum SortOrder
{
    ASC,
    DESC
}
