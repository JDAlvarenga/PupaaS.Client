namespace PupaaS.Client.Services;

public class QueryParameters
{
    public static int DefaultPageSize = 10;
    public static int DefaultPageNumber = 1;
    
    public string? Search { get; set; }


    private int _page = 1;

    public int Page
    {
        get => _page;
        set => _page = int.Max(value, DefaultPageNumber);
    }

    private int _pageSize = DefaultPageSize;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = int.Max(value, 1);
    }
}