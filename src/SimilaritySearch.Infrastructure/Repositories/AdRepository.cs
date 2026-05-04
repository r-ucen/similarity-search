using Microsoft.EntityFrameworkCore;
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