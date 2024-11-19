namespace PupaaS.Client.Services;

public class PupusaParameters : QueryParameters
{
    public string? Dough { get; set; }
    public IEnumerable<string>? Ingredients { get; set; }
}