namespace SimilaritySearch.Application.Abstractions;

public interface IUserContext
{
    Task<string> GetCurrentUserIdAsync();
    Task<bool> IsUserAuthenticatedAsync();
    Task<bool> IsInRoleAsync(string role);
}