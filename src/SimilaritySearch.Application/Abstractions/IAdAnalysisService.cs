using SimilaritySearch.Application.DTOs;

namespace SimilaritySearch.Application.Abstractions;

public interface IAdAnalysisService
{
    public Task AnalyseAdAsync(AdDto ad, CancellationToken ct);
    public Task CreateEmbedding(AdDto ad, CancellationToken ct);
    public Task CreateAiReason(AdDto newAd, AdDto? oldAd, CancellationToken ct);
}