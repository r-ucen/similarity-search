using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;
using SimilaritySearch.Application.Abstractions.Repositories;
using SimilaritySearch.Application.DTOs;
using SimilaritySearch.Application.Extensions.Ad;
using SimilaritySearch.Domain.Entities;
using SimilaritySearch.Infrastructure.Database;

namespace SimilaritySearch.Infrastructure.Repositories;

public class AdRepository : Repository<Ad>, IAdRepository
{
    public AdRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Tuple<Ad, double>?> GetMostSimilarAdAsync(Guid adId, CancellationToken ct)
    {
        var ad = await DbSet.FirstOrDefaultAsync(a => a.Id == adId, cancellationToken: ct);
        if (ad == null) { throw new InvalidOperationException($"Ad with id: {adId} was not found"); }
        if (ad.DescriptionEmbedding == null) { return null; }

        return await DbSet
            .Where(x => x.Id != ad.Id && x.DescriptionEmbedding != null)
            .OrderBy(x => x.DescriptionEmbedding!.CosineDistance(ad.DescriptionEmbedding!))
            .Select(x => new Tuple<Ad, double>(x, x.DescriptionEmbedding!.CosineDistance(ad.DescriptionEmbedding!)))
            .FirstOrDefaultAsync(cancellationToken: ct);
    }
    
    public async Task<IEnumerable<AdDto>?> GetAllAdsAsync()
    {
        return await DbSet
            .AsNoTracking()
            .Where(a => !a.IsDeleted && a.ReadyToBePresented)
            .Select(a => a.DtoFromEntity())
            .ToListAsync();
    }
    
    public async Task<AdDto?> GetAdAsync(Guid id)
    {
        return await DbSet
            .AsNoTracking()
            .Where(a => a.Id == id && !a.IsDeleted && a.ReadyToBePresented)
            .Select(a => a.DtoFromEntity())
            .FirstOrDefaultAsync();
    }
}