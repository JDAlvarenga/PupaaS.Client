namespace PupaaS.Client.Services;

public class PupusaParameters : QueryParameters
{
    public const string DoughParamName = "masa";
    public const string IngredientsParamName = "ing";
    public string? Dough { get; set; }
    public IEnumerable<string>? Ingredients { get; set; }
}