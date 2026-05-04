using SimilaritySearch.Application.DTOs;

namespace SimilaritySearch.Application.Abstractions;

public interface IBackgroundJobService
{
    public void EnqueueAdAnalysisAsync(AdDto ad);
}