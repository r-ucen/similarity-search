using Pgvector;
using SimilaritySearch.Application.DTOs;
using SimilaritySearch.Domain.Entities;

namespace SimilaritySearch.Application.Abstractions.Repositories;

public interface IAdRepository
{
    Task<IEnumerable<AdDto>?> GetAllAdsAsync();
    public Task SetEmbeddingAsync(Guid adId, Vector embedding);
    public Task SetReuploadAsync(Guid adId, bool reupload);
    public Task SetReuploadReasonAsync(Guid adId, string reason);
    public Task<Tuple<Ad, double>?> GetMostSimilarAdAsync(Guid adId, CancellationToken ct);
    public Task SetReadyToBePresentedAsync(Guid adId, bool readyToBePresented);
    Task<AdDto?> GetAdAsync(Guid id);
    Task<int> CreateAdAsync(Ad ad);
    Task<int> UpdateAdAsync(Guid adId, EditAdCommand updatedAd);
    Task<int> DeleteAdAsync(Guid id);
}