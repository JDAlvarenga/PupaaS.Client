using PupaaS.Client.Models;

namespace PupaaS.Client.Services.Mock;

public class MockPupusaService: IPupusaService
{
    private List<Pupusa> _pupusas = new();
    

    public MockPupusaService()
    {
        for (int i = 0; i < 50; i++)
        {
            _pupusas.Add(new()
            {
                Url = $"https://picsum.photos/500?random={i}",
                Dough = AppParameters.DoughTypes[Random.Shared.Next(AppParameters.DoughTypes.Length)],
                Ingredients = AppParameters.Ingredients.OrderBy(ing => Random.Shared.Next()).Take(Random.Shared.Next(1, 4)).ToArray()
            });
        }
    }
    public Task<PagedEnumerable<Pupusa>?> GetPupusasAsync(PupusaParameters pupusaParameters, CancellationToken cancellationToken = default)
    {
        var query = _pupusas.AsQueryable();
        if (!string.IsNullOrEmpty(pupusaParameters.Search))
        {
            query = query.Where(p => 
                (p.Dough != null && p.Dough.Contains(pupusaParameters.Search)) || 
                (p.Ingredients != null && p.Ingredients.Any( i => i.Contains(pupusaParameters.Search)))
            );
        }
        else
        {
            if (pupusaParameters.Dough is not null)
                query = query.Where(p => p.Dough == pupusaParameters.Dough);
            
            if (pupusaParameters.Ingredients is not null && pupusaParameters.Ingredients.Any())
                query = query.Where(p => p.Ingredients != null && p.Ingredients.Any(i => AppParameters.Ingredients.Contains(i)));
        }
        
        
        var results = query.ToList();
        
        
        var pagedItems = query
            .Skip((pupusaParameters.Page - 1) * pupusaParameters.PageSize)
            .Take(pupusaParameters.PageSize)
            .ToList();
        
        return Task.FromResult<PagedEnumerable<Pupusa>?>(
            new PagedEnumerable<Pupusa>
            {
                Page = pupusaParameters.Page,
                Size = pupusaParameters.PageSize,
                TotalItems = results.Count,
                Items = pagedItems,
            });
    }
}