using SimilaritySearch.Application.Abstractions.Repositories;

namespace SimilaritySearch.Application.Abstractions;

public interface IUnitOfWork : IDisposable
{
    IAdRepository Ads { get; }
    
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync();
}