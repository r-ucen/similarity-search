using Microsoft.Extensions.AI;
using Pgvector;
using SimilaritySearch.Application.Abstractions;
using SimilaritySearch.Application.DTOs;

namespace SimilaritySearch.Infrastructure.Services;

public class TextAnalysisService : ITextAnalysisService
{
    private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator;
    private readonly IChatClient _chatClient;
    
    public TextAnalysisService(IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator, IChatClient chatClient)
    {
        _embeddingGenerator = embeddingGenerator;
        _chatClient = chatClient;
    }
    
    public async Task<Vector> GenerateTextEmbeddingAsync(string text, CancellationToken ct)
    {
        var embedding = await _embeddingGenerator.GenerateAsync(text, cancellationToken: ct);
        return new Vector(embedding.Vector);
    }

    public async Task<string> AnalyzeDuplicateAsync(AdDto newAd, AdDto oldAd, string hint, CancellationToken ct)
    {
        var prompt = $"""
                      Porovnej tyto dva inzeráty:
                      Inzerát A: {newAd.Summarize()}
                      Inzerát B: {oldAd.Summarize()}

                      Je inzerát A pravděpodobně re-uploadem inzerátu B?
                      {hint}
                      Zaměř se na detaily jako výbava, specifické chyby nebo styl psaní.
                      Odpověz ve formátu: ROZHODNUTÍ: [ANO/NE] | DŮVOD: [Stručné vysvětlení a napsání rozdílů: zda je rozdíl mezi userId (uživatel vytvořil nový účet), lokace, cena, rozdíl mezi daty vytvoření]
                      """;
        
        var response = await _chatClient.GetResponseAsync(prompt, cancellationToken: ct);
        return response.Text;
    }
}