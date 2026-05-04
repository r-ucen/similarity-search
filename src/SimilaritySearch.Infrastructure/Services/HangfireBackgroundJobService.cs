using Hangfire;
using SimilaritySearch.Application.Abstractions;
using SimilaritySearch.Application.DTOs;

namespace SimilaritySearch.Infrastructure.Services;

public class HangfireBackgroundJobService : IBackgroundJobService
{
    public void EnqueueAdAnalysisAsync(AdDto ad)
    {
        BackgroundJob.Enqueue<IAdAnalysisService>(s => s.AnalyseAdAsync(ad, CancellationToken.None));
    }
}