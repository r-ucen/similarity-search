using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;
using SimilaritySearch.Application.Abstractions.Repositories;
using SimilaritySearch.Application.DTOs;
using SimilaritySearch.Domain.Entities;
using SimilaritySearch.Infrastructure.Database;

namespace SimilaritySearch.Infrastructure.Repositories;

public class AdRepository : IAdRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    
    public AdRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task SetEmbeddingAsync(Guid adId, Vector embedding)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var ad = await context.Ads.FirstOrDefaultAsync(a => a.Id == adId);

        if (ad == null)
        {
            throw new InvalidOperationException($"Ad with id: {adId} was not found");
        }

        ad.DescriptionEmbedding = embedding;
        await context.SaveChangesAsync();
    }

    public async Task SetReuploadAsync(Guid adId, bool reupload)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var ad = await context.Ads.FirstOrDefaultAsync(a => a.Id == adId);
        if (ad == null)
        {
            throw new InvalidOperationException($"Ad with id: {adId} was not found");
        }
        ad.IsReupload = reupload;
        await context.SaveChangesAsync(); 
    }

    public async Task SetReuploadReasonAsync(Guid adId, string reason)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var ad = await context.Ads.FirstOrDefaultAsync(a => a.Id == adId);
        if (ad == null)
        {
            throw new InvalidOperationException($"Ad with id: {adId} was not found");
        }
        ad.ReuploadReason = reason;
        await context.SaveChangesAsync();
    }

    public async Task<Tuple<Ad, double>?> GetMostSimilarAdAsync(Guid adId, CancellationToken ct)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(ct);
        var ad = await context.Ads.FirstOrDefaultAsync(a => a.Id == adId, cancellationToken: ct);
        if (ad == null)
        {
            throw new InvalidOperationException($"Ad with id: {adId} was not found");
        }

        return await context.Ads
            .Where(x => x.Id != ad.Id)
            .OrderBy(x => x.DescriptionEmbedding!.CosineDistance(ad.DescriptionEmbedding!))
            .Select(x => new Tuple<Ad, double>(x, x.DescriptionEmbedding!.CosineDistance(ad.DescriptionEmbedding!)))
            .FirstOrDefaultAsync(cancellationToken: ct);
    }

    public async Task SetReadyToBePresentedAsync(Guid adId, bool readyToBePresented)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var ad = await context.Ads.FirstOrDefaultAsync(a => a.Id == adId);
        if (ad == null)
        {
            throw new InvalidOperationException($"Ad with id: {adId} was not found");
        }
        ad.ReadyToBePresented =  readyToBePresented;
        await context.SaveChangesAsync();
    }
    
    public async Task<IEnumerable<AdDto>?> GetAllAdsAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Ads
            .AsNoTracking()
            .Where(a => !a.IsDeleted && a.ReadyToBePresented)
            .Select(a => new AdDto
            {
                Id = a.Id,
                UserId = a.UserId,
                UserName = a.UserName,
                BrandModel = a.BrandModel,
                Motor = a.Motor,
                PhoneNumber = a.PhoneNumber,
                Email = a.Email,
                Description = a.Description,
                Currency = a.Currency,
                Location = a.Location,
                Price = a.Price,
                IsReupload = a.IsReupload,
                ReuploadReason = a.ReuploadReason
            })
            .ToListAsync();
    }
    
    public async Task<AdDto?> GetAdAsync(Guid id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Ads
            .AsNoTracking()
            .Where(a => a.Id == id && !a.IsDeleted && a.ReadyToBePresented)
            .Select(a => new AdDto
            {
                Id = a.Id,
                UserId = a.UserId,
                UserName = a.UserName,
                BrandModel = a.BrandModel,
                Motor = a.Motor,
                PhoneNumber = a.PhoneNumber,
                Email = a.Email,
                Description = a.Description,
                Currency = a.Currency,
                Location = a.Location,
                Price = a.Price,
                IsReupload = a.IsReupload,
                ReuploadReason = a.ReuploadReason
            })
            .FirstOrDefaultAsync();
    }
    
    public async Task<int> CreateAdAsync(Ad ad)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        context.Ads.Add(ad);
        return await context.SaveChangesAsync();
    }
    
    public async Task<int> UpdateAdAsync(Guid adId, EditAdCommand updatedAd)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var existingAd = await context.Ads.FirstOrDefaultAsync(a => a.Id == adId);

        if (existingAd == null)
        {
            throw new InvalidOperationException($"Ad with id: {existingAd} was not found");
        }
        
        existingAd.UserName = updatedAd.UserName;
        existingAd.BrandModel = updatedAd.BrandModel;
        existingAd.Motor = updatedAd.Motor;
        existingAd.PhoneNumber = updatedAd.PhoneNumber;
        existingAd.Email = updatedAd.Email;
        existingAd.Description = updatedAd.Description;
        existingAd.Location = updatedAd.Location;
        existingAd.Price = updatedAd.Price;
        existingAd.Description = updatedAd.Description;
        
        return await context.SaveChangesAsync();
    }
    
    public async Task<int> DeleteAdAsync(Guid id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var ad = await context.Ads.FirstOrDefaultAsync(a => a.Id == id);

        if (ad == null)
        {
            throw new InvalidOperationException($"Ad with id: {id} was not found");
        }
        
        ad.IsDeleted = true;
        return await context.SaveChangesAsync();
    }
}