using SimilaritySearch.Application.DTOs;

namespace SimilaritySearch.Application.Extensions.Ad;

public static class AdExtension
{
    extension(Domain.Entities.Ad ad)
    {
        public AdDto DtoFromEntity()
        {
            return new AdDto()
            {
                Id = ad.Id,
                UserId = ad.UserId,
                UserName = ad.UserName,
                BrandModel = ad.BrandModel,
                Motor = ad.Motor,
                PhoneNumber = ad.PhoneNumber,
                Email = ad.Email,
                Description = ad.Description,
                Location = ad.Location,
                Price = ad.Price,
                Currency = ad.Currency,
                IsReupload = ad.IsReupload,
                ReuploadReason = ad.ReuploadReason,
                CreatedAt = ad.CreatedAt
            };
        }
    }
}