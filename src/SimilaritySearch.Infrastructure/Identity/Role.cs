using Microsoft.AspNetCore.Identity;
namespace SimilaritySearch.Infrastructure.Identity;

public class Role : IdentityRole
{
    public Role(string role) : base(role) { }
    public Role() : base() { }
}