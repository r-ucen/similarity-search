using SimilaritySearch.Application.DTOs;

namespace SimilaritySearch.Application.Abstractions.Repositories;

public interface IUserRepository
{
    Task<List<UserDto>> GetAllAsync();
    Task<UserDto> GetByIdAsync(string id);
    Task<bool> DeleteAsync(string userId);
}