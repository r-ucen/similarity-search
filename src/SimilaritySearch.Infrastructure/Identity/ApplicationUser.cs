using System;
using Microsoft.AspNetCore.Identity;
using SimilaritySearch.Domain.Entities.Interfaces;
using SimilaritySearch.Domain.Entities;

namespace SimilaritySearch.Infrastructure.Identity;

public sealed class ApplicationUser : IdentityUser<string>, IUser<string>
{
    public ApplicationUser() { Id = Guid.NewGuid().ToString(); }
}