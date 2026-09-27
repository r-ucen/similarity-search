using SimilaritySearch.Application.DTOs;

namespace SimilaritySearch.Application.Abstractions;

public interface IIdentityService
{
    Task LogOutAsync();
    public Task<bool> DeleteAsync(string userId);
    public Task<List<UserDto>> GetAllAsync();
    public Task<UserDto> GetByIdAsync(string id);
}