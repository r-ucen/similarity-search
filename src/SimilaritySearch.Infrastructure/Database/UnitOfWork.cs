using Microsoft.AspNetCore.Identity;
using SimilaritySearch.Application.Abstractions;
using SimilaritySearch.Application.Abstractions.Repositories;
using SimilaritySearch.Infrastructure.Database;
using SimilaritySearch.Infrastructure.Identity;
using SimilaritySearch.Infrastructure.Repositories;

public class UnitOfWork: IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    private IAdRepository _ads;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IAdRepository Ads => 
        _ads ??= new AdRepository(_context);

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RollbackAsync()
    {
        await _context.DisposeAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}