using SimilaritySearch.Application.Abstractions;

namespace SimilaritySearch.Infrastructure.Services;

public class LevenshteinService : ILevenshteinService
{
    public int CalculateDistance(string s1, string s2)
    {
        return Fastenshtein.Levenshtein.Distance(s1, s2);
    }
}