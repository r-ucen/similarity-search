using System.Net;

namespace SimilaritySearch.Application.Exceptions.Ad;

public class AdCreationFailedException : AppException
{
    public AdCreationFailedException(string message) : base(message, 500)
    {
    }
}