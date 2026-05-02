namespace SimilaritySearch.Application.Exceptions.Ad;

public class AdNotFoundException : AppException
{
    public AdNotFoundException(string message) : base(message, 404)
    {
    }
}