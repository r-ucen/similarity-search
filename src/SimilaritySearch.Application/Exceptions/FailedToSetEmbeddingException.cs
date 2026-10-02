namespace SimilaritySearch.Application.Exceptions;

public class FailedToSetEmbeddingException : AppException
{
    public FailedToSetEmbeddingException(string message) : base(message, 500) { }
}