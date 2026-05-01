using Microsoft.AspNetCore.Identity;
using StockMapSvelte.Domain.Entities;
using StockMapSvelte.Domain.Entities.Interfaces;

namespace StockMapSvelte.Infrastructure.Identity;

public sealed class ApplicationUser : IdentityUser<string>, IUser<string>
{
    public ApplicationUser() { Id = Guid.NewGuid().ToString(); }
}