using StockMapSvelte.Application.DTOs;

namespace StockMapSvelte.Application.Abstractions.Repositories;

public interface IUserRepository
{
    Task<List<UserDto>> GetAllAsync();
    Task<UserDto> GetByIdAsync(string id);
    Task<bool> DeleteAsync(string userId);
}