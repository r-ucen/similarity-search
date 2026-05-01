namespace StockMapSvelte.Application.Exceptions.User;

public class UserNotFoundException : AppException
{
    public UserNotFoundException(string message) : base(message, 404) { }
}