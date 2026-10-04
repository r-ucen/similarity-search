using SimilaritySearch.Application.DTOs;

namespace SimilaritySearch.Application.Abstractions;

public interface IAdAnalysisService
{
    public Task AnalyseAdAsync(AdDto ad, CancellationToken ct);
}