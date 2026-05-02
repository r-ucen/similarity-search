namespace SimilaritySearch.Application.Exceptions.Ad;

public class AdDeleteFailedException : AppException
{
    public AdDeleteFailedException(string message) : base(message, 500)
    {
    }
}