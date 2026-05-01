using StockMapSvelte.Infrastructure.Identity;

namespace StockMapSvelte.Infrastructure.Database.Seeding;

internal class RolesInit
{
    public List<Role> GetRolesAm()
    {
        List<Role> roles = new List<Role>();

        Role roleAdmin = new Role()
        {
            Id = "1",
            Name = "Admin",
            NormalizedName = "ADMIN",
            ConcurrencyStamp = "9cf14c2c-19e7-40d6-b744-8917505c3106"
        };

        Role roleManager = new Role()
        {
            Id = "2",
            Name = "Manager",
            NormalizedName = "MANAGER",
            ConcurrencyStamp = "be0efcde-9d0a-461d-8eb6-444b043d6660"
        };

        roles.Add(roleAdmin);
        roles.Add(roleManager);

        return roles;
    }
}