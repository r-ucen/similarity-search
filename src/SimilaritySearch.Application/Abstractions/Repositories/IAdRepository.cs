using Pgvector;
using SimilaritySearch.Application.DTOs;
using SimilaritySearch.Domain.Entities;

namespace SimilaritySearch.Application.Abstractions.Repositories;

public interface IAdRepository : IRepository<Ad>
{
    Task<IEnumerable<AdDto>?> GetAllAdsAsync();
    public Task<Tuple<Ad, double>?> GetMostSimilarAdAsync(Guid adId, CancellationToken ct);
    Task<AdDto?> GetAdAsync(Guid id);
}