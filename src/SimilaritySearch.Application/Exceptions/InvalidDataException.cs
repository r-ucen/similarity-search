namespace SimilaritySearch.Application.Exceptions;

public class InvalidDataException : AppException
{
    public InvalidDataException(string message) : base(message, 500) { }
}