namespace PupaaS.Client.Models;

public class Pupusa:ICloneable
{
    public string Url { get; set; } = string.Empty;
    public string? Dough { get; set; }
    public IEnumerable<string>? Ingredients { get; set; }
    public object Clone()
    {
        return new Pupusa()
        {

            Url = Url,
            Dough = Dough,
            Ingredients = this.Ingredients?.ToList()
        };
    }
}

