namespace SimilaritySearch.Application.Exceptions.User;

public class DeleteUserFailException : AppException
{
    public DeleteUserFailException(string message) : base(message, 500) { }
}