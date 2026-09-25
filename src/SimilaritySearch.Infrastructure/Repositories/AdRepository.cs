using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;
using SimilaritySearch.Application.Abstractions.Repositories;
using SimilaritySearch.Application.DTOs;
using SimilaritySearch.Domain.Entities;
using SimilaritySearch.Infrastructure.Database;

namespace SimilaritySearch.Infrastructure.Repositories;

public class AdRepository : Repository<Ad>, IAdRepository
{
    private readonly ApplicationDbContext _context;
    
    public AdRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task SetEmbeddingAsync(Guid adId, Vector embedding)
    {
        var ad = await DbSet.FirstOrDefaultAsync(a => a.Id == adId);

        if (ad == null)
        {
            throw new InvalidOperationException($"Ad with id: {adId} was not found");
        }

        ad.DescriptionEmbedding = embedding;
        await _context.SaveChangesAsync();
    }

    public async Task SetReuploadAsync(Guid adId, bool reupload)
    {
        var ad = await DbSet.FirstOrDefaultAsync(a => a.Id == adId);
        if (ad == null)
        {
            throw new InvalidOperationException($"Ad with id: {adId} was not found");
        }
        ad.IsReupload = reupload;
        await _context.SaveChangesAsync(); 
    }

    public async Task SetReuploadReasonAsync(Guid adId, string reason)
    {
        var ad = await DbSet.FirstOrDefaultAsync(a => a.Id == adId);
        if (ad == null)
        {
            throw new InvalidOperationException($"Ad with id: {adId} was not found");
        }
        ad.ReuploadReason = reason;
        await _context.SaveChangesAsync();
    }

    public async Task<Tuple<Ad, double>?> GetMostSimilarAdAsync(Guid adId, CancellationToken ct)
    {
        var ad = await DbSet.FirstOrDefaultAsync(a => a.Id == adId, cancellationToken: ct);
        if (ad == null)
        {
            throw new InvalidOperationException($"Ad with id: {adId} was not found");
        }

        return await DbSet
            .Where(x => x.Id != ad.Id)
            .OrderBy(x => x.DescriptionEmbedding!.CosineDistance(ad.DescriptionEmbedding!))
            .Select(x => new Tuple<Ad, double>(x, x.DescriptionEmbedding!.CosineDistance(ad.DescriptionEmbedding!)))
            .FirstOrDefaultAsync(cancellationToken: ct);
    }

    public async Task SetReadyToBePresentedAsync(Guid adId, bool readyToBePresented)
    {
        var ad = await DbSet.FirstOrDefaultAsync(a => a.Id == adId);
        if (ad == null)
        {
            throw new InvalidOperationException($"Ad with id: {adId} was not found");
        }
        ad.ReadyToBePresented =  readyToBePresented;
        await _context.SaveChangesAsync();
    }
    
    public async Task<IEnumerable<AdDto>?> GetAllAdsAsync()
    {
        return await DbSet
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
        return await DbSet
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
        DbSet.Add(ad);
        return await _context.SaveChangesAsync();
    }
    
    public async Task<int> UpdateAdAsync(Guid adId, EditAdCommand updatedAd)
    {
        var existingAd = await DbSet.FirstOrDefaultAsync(a => a.Id == adId);

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
        
        return await _context.SaveChangesAsync();
    }
    
    public async Task<int> DeleteAdAsync(Guid id)
    {
        var ad = await DbSet.FirstOrDefaultAsync(a => a.Id == id);

        if (ad == null)
        {
            throw new InvalidOperationException($"Ad with id: {id} was not found");
        }
        
        ad.IsDeleted = true;
        return await _context.SaveChangesAsync();
    }
}