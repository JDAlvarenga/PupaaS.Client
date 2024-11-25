using System.Net.Http.Json;
using System.Text.Json;
using System.Web;
using PupaaS.Client.Models;

namespace PupaaS.Client.Services;

public class PupusaService(HttpClient client, string baseUrl): IPupusaService
{
    public async Task<PagedEnumerable<Pupusa>?> GetPupusasAsync(PupusaParameters pupusaParameters, CancellationToken cancellationToken = default)
    {
        // Build query
        var url = ParametersToQueryString(pupusaParameters);
        
        // Send query
        var response = await client.GetAsync(url, cancellationToken);
        if (!response.IsSuccessStatusCode)
            return null; // TODO: Actually handle the error
        
        var pupusas = await response.Content.ReadFromJsonAsync<List<Pupusa>>(cancellationToken);
        if (pupusas == null) return null;
        
        var metadata = JsonSerializer.Deserialize<PageMetadata>(response.Headers.GetValues("X-Pagination").First());
        
        return new PagedEnumerable<Pupusa>
        {
            Page = metadata?.CurrentPage ?? 1,
            Size = metadata?.PageSize ?? pupusas.Count,
            TotalItems = metadata?.TotalCount ?? pupusas.Count,
            Items = pupusas,
        };
    }

    public async Task<Pupusa?> AddPupusaAsync(NewPupusa newPupusa, CancellationToken cancellationToken = default)
    {
        if (newPupusa.File is null) return null;
        
        await using var fileStream = newPupusa.File.OpenReadStream();
        using var content = new MultipartFormDataContent();
        content.Add(new StreamContent(fileStream), "file", DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"));
        content.Add(new StringContent(newPupusa.Dough ?? String.Empty), PupusaParameters.DoughParamName);

        foreach (var ingredient in newPupusa.Ingredients ?? [])
        {
            content.Add(new StringContent(ingredient), PupusaParameters.IngredientsParamName);
        }
        var response = await client.PostAsync(baseUrl, content, cancellationToken);
        if (!response.IsSuccessStatusCode)
            return null;
        
        
        return await response.Content.ReadFromJsonAsync<Pupusa>(cancellationToken);
    }
    
    public async Task<Pupusa?> UpdatePupusaAsync(Pupusa pupusa, CancellationToken cancellationToken = default)
    {
        var response = await client.PutAsJsonAsync(baseUrl, pupusa, cancellationToken);
        if (!response.IsSuccessStatusCode)
            return null;
        
        return await response.Content.ReadFromJsonAsync<Pupusa>(cancellationToken);
    }

    public async Task<bool> DeletePupusaAsync(Pupusa pupusa , CancellationToken cancellationToken = default)
    {
        // TODO: Pendiente de definir id de la imagen
        var response = await client.DeleteAsync($"{baseUrl}/{pupusa.Url}", cancellationToken);
        return response.IsSuccessStatusCode;
    }
    

    private string ParametersToQueryString(PupusaParameters pupusaParameters)
    {
        var query = HttpUtility.ParseQueryString(String.Empty);
        if (!string.IsNullOrEmpty(pupusaParameters.Dough))
            query.Add(PupusaParameters.DoughParamName, pupusaParameters.Dough);
        
        foreach (var ingredient in pupusaParameters.Ingredients ?? [])
        {
            query.Add(PupusaParameters.IngredientsParamName, ingredient);
        }

        return $"{baseUrl}?{query}";
    }
    
}