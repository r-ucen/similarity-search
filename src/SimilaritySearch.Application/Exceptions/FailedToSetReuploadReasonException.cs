namespace SimilaritySearch.Application.Exceptions;

public class FailedToSetReuploadReasonException : AppException
{
    public FailedToSetReuploadReasonException(string message) : base(message, 500) { }
}