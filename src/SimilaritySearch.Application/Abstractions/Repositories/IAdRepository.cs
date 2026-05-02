using SimilaritySearch.Application.DTOs;
using SimilaritySearch.Domain.Entities;

namespace SimilaritySearch.Application.Abstractions.Repositories;

public interface IAdRepository
{
    Task<IEnumerable<AdDto>?> GetAllAdsAsync();
    Task<AdDto?> GetAdAsync(Guid id);
    Task<int> CreateAdAsync(Ad ad);
    Task<int> UpdateAdAsync(Guid adId, EditAdCommand updatedAd);
    Task<int> DeleteAdAsync(Guid id);
}