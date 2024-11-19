namespace PupaaS.Client.Models;

public class PagedEnumerable<T>
{
    public int Page { get; set; }
    public int Size { get; set; }
    public int TotalItems { get; set; }
    
    public required IEnumerable<T> Items { get; set; }
}