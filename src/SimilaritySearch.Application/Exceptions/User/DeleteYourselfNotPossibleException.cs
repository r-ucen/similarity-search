namespace StockMapSvelte.Application.Exceptions.User;

public class DeleteYourselfNotPossibleException : AppException
{
    public DeleteYourselfNotPossibleException(string message) : base(message, 400)
    {
    }
}