using PupaaS.Client.Models;

namespace PupaaS.Client.Services.Mock;

public class MockPupusaService: IPupusaService
{
    private Dictionary<string, Pupusa> _pupusas = new();
    private int _nextId = 1;
    private string NextMockUrl => $"https://picsum.photos/500?random={_nextId++}";
    

    public MockPupusaService()
    {
        while(_nextId <= 50)
        {
            var url = NextMockUrl;
            _pupusas.Add(url, new()
            {
                Url = url,
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
            query = query.Where(kvp => 
                (kvp.Value.Dough != null && kvp.Value.Dough.Contains(pupusaParameters.Search)) || 
                (kvp.Value.Ingredients != null && kvp.Value.Ingredients.Any( i => i.Contains(pupusaParameters.Search)))
            );
        }
        
        
        if (pupusaParameters.Dough is not null)
            query = query.Where(kvp => kvp.Value.Dough == pupusaParameters.Dough);
        
        if (pupusaParameters.Ingredients is not null && pupusaParameters.Ingredients.Any())
            query = query.Where(kvp => kvp.Value.Ingredients != null && kvp.Value.Ingredients.Any(i => pupusaParameters.Ingredients.Contains(i)));
        
        
        
        var results = query.Select(kvp => kvp.Value).ToList();
        
        
        var pagedItems = results
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

    public async Task<Pupusa?> AddPupusaAsync(NewPupusa newPupusa, CancellationToken cancellationToken = default)
    {
        Pupusa pupusa = new()
        {
            Url = NextMockUrl,
            Dough = newPupusa.Dough,
            Ingredients = newPupusa.Ingredients
        };
        await Task.Delay(TimeSpan.FromSeconds(Random.Shared.NextDouble() * 7), cancellationToken);
        _pupusas.Add(pupusa.Url, pupusa);
        
        return pupusa.Clone() as Pupusa;
    }

    public async Task<Pupusa?> UpdatePupusaAsync(Pupusa pupusa, CancellationToken cancellationToken = default)
    {
        await Task.Delay(TimeSpan.FromSeconds(Random.Shared.NextDouble() * 7), cancellationToken);

        if (!_pupusas.ContainsKey(pupusa.Url))
            return null;
        
        _pupusas[pupusa.Url] = (pupusa.Clone() as Pupusa)!;

        return pupusa;
    }

    public async Task<bool> DeletePupusaAsync(Pupusa pupusa, CancellationToken cancellationToken = default)
    {
        await Task.Delay(TimeSpan.FromSeconds(Random.Shared.NextDouble() * 7), cancellationToken);
        _pupusas.Remove(pupusa.Url);
        return true;
    }
    
}