using Pgvector;
using SimilaritySearch.Application.DTOs;

namespace SimilaritySearch.Application.Abstractions;

public interface ITextAnalysisService
{
    public Task<Vector> GenerateTextEmbeddingAsync(string text, CancellationToken ct);
    public Task<string> AnalyzeDuplicateAsync(AdDto newAd, AdDto? oldAd, CancellationToken ct);
}