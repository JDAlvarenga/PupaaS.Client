using PupaaS.Client.Models;

namespace PupaaS.Client.Services;

public interface IPupusaService
{
    Task<PagedEnumerable<Pupusa>?> GetPupusasAsync(PupusaParameters pupusaParameters,
        CancellationToken cancellationToken = default);
    
    Task<Pupusa?> AddPupusaAsync(NewPupusa newPupusa, CancellationToken cancellationToken = default);
    Task<Pupusa?> UpdatePupusaAsync(Pupusa pupusa, CancellationToken cancellationToken = default);
}