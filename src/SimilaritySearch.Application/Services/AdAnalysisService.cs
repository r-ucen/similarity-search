using SimilaritySearch.Application.Abstractions;
using SimilaritySearch.Application.Abstractions.Repositories;
using SimilaritySearch.Application.DTOs;

namespace SimilaritySearch.Application.Services;

public class AdAnalysisService : IAdAnalysisService
{
    private readonly ITextAnalysisService _textAnalysisService;
    private readonly IAdRepository _adRepository;
    private readonly ILevenshteinService _levenshteinService;
    
    public AdAnalysisService(ITextAnalysisService textAnalysisService, IAdRepository adRepository, ILevenshteinService levenshteinService)
    {
        _textAnalysisService = textAnalysisService;
        _adRepository = adRepository;
        _levenshteinService = levenshteinService;
    }
    
    private static string NormalizeForComparison(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;
        return input.ToLowerInvariant()
            .Replace(" ", "")
            .Replace(",", ".")
            .Trim();
    }

    public async Task AnalyseAdAsync(AdDto ad, CancellationToken ct)
    {
        await CreateEmbedding(ad, ct);
        var mostSimilarAd = await _adRepository.GetMostSimilarAdAsync(ad.Id, ct);

        if (mostSimilarAd == null)
        {
            await _adRepository.SetReuploadAsync(ad.Id, false);
            await _adRepository.SetReuploadReasonAsync(ad.Id, "Nebyl nalezen žádný podobný inzerát k porovnání.");
            await _adRepository.SetReadyToBePresentedAsync(ad.Id, true);
            return;
        }
        
        const double threshold = 0.35;
        var isReupload = false;

        isReupload = mostSimilarAd.Item2 < threshold;

        if (isReupload)
        {
            var brandModelDistance = _levenshteinService.CalculateDistance(NormalizeForComparison(ad.BrandModel), NormalizeForComparison(mostSimilarAd.Item1.BrandModel));
            var motorDistance = _levenshteinService.CalculateDistance(NormalizeForComparison(ad.Motor), NormalizeForComparison(mostSimilarAd.Item1.Motor));
            
            if (brandModelDistance >= 2 || motorDistance >= 2)
            {
                isReupload = false;
                await _adRepository.SetReuploadReasonAsync(ad.Id, "Inzerát je unikátní (Rozdíl v modelu či typu motoru)");
            }
            else
            {
                var mostSimilarAdDto = await _adRepository.GetAdAsync(mostSimilarAd.Item1.Id);
                await CreateAiReason(ad, mostSimilarAdDto, ct);
            }
        }
        else
        {
            await _adRepository.SetReuploadReasonAsync(ad.Id, "Inzerát je unikátní");
        }
        
        await _adRepository.SetReuploadAsync(ad.Id, isReupload);
        await _adRepository.SetReadyToBePresentedAsync(ad.Id, true);
    }
    
    public async Task CreateEmbedding(AdDto ad, CancellationToken ct)
    {
        var embedding = await _textAnalysisService.GenerateTextEmbeddingAsync(ad.Description, ct);
        await _adRepository.SetEmbeddingAsync(ad.Id, embedding);
    }

    public async Task CreateAiReason(AdDto newAd, AdDto? oldAd, CancellationToken ct)
    {
        var reason = await _textAnalysisService.AnalyzeDuplicateAsync(newAd, oldAd, ct);
        await _adRepository.SetReuploadReasonAsync(newAd.Id, reason);
    }
}