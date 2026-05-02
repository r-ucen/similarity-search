namespace SimilaritySearch.Application.Exceptions.Ad;

public class AdEditFailedException : AppException
{
    public AdEditFailedException(string message) : base(message, 500) { }
}