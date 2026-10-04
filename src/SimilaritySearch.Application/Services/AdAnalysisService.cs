using SimilaritySearch.Application.Abstractions;
using SimilaritySearch.Application.DTOs;
using SimilaritySearch.Application.Exceptions.Ad;

namespace SimilaritySearch.Application.Services;

public class AdAnalysisService : IAdAnalysisService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITextAnalysisService _textAnalysisService;
    private readonly ILevenshteinService _levenshteinService;
    
    public AdAnalysisService(IUnitOfWork unitOfWork, ITextAnalysisService textAnalysisService, ILevenshteinService levenshteinService)
    {
        _unitOfWork = unitOfWork;
        _textAnalysisService = textAnalysisService;
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
        var originalAd = await _unitOfWork.Ads.GetByIdAsync(ad.Id, ct);
        if (originalAd == null) { throw new AdNotFoundException($"Ad with id: {ad.Id} was not found"); }
        
        var embedding = await _textAnalysisService.GenerateTextEmbeddingAsync(ad.Description, ct);
        originalAd.DescriptionEmbedding = embedding;

        var mostSimilarAd = await _unitOfWork.Ads.GetMostSimilarAdAsync(ad.Id, ct);

        if (mostSimilarAd == null)
        {
            originalAd.IsReupload = false;
            originalAd.ReuploadReason = "Nebyl nalezen žádný podobný inzerát k porovnání.";
            originalAd.ReadyToBePresented = true;

            await _unitOfWork.CommitAsync(ct);
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
                originalAd.ReuploadReason = "Inzerát je unikátní (Rozdíl v modelu či typu motoru)";
            }
            else
            {
                var mostSimilarAdDto = await _unitOfWork.Ads.GetAdAsync(mostSimilarAd.Item1.Id);
                
                var reason = await _textAnalysisService.AnalyzeDuplicateAsync(ad, mostSimilarAdDto, ct);
                originalAd.ReuploadReason = reason;
            }
        }
        else
        {
            originalAd.ReuploadReason = "Inzerát je unikátní";
        }
        
        originalAd.IsReupload = isReupload;
        originalAd.ReadyToBePresented = true;
        
        await _unitOfWork.CommitAsync(ct);
    }
}