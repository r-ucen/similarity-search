namespace SimilaritySearch.Application.Exceptions.Ad;

public class DataMissingException : AppException
{
    public DataMissingException(string message) : base(message, 400) { }
}