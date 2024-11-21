using Microsoft.AspNetCore.Components.Forms;

namespace PupaaS.Client.Models;

public class NewPupusa
{
    public IBrowserFile? File { get; set; }
    public string? Dough { get; set; }
    public IEnumerable<string>? Ingredients { get; set; }
}