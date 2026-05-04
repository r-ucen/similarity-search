namespace SimilaritySearch.Application.Abstractions;

public interface ILevenshteinService
{
    public int CalculateDistance(string s1, string s2);
}