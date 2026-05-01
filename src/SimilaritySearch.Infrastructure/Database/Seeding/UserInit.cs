using StockMapSvelte.Infrastructure.Identity;

namespace StockMapSvelte.Infrastructure.Database.Seeding;

internal class UserInit
{
    public ApplicationUser GetAdmin()
    {
        ApplicationUser admin = new ApplicationUser()
        {
            Id = "702f52fd-f23b-462d-b84b-0e1acc17785c",
            UserName = "admin@rucen.me",
            NormalizedUserName = "ADMIN@RUCEN.ME",
            Email = "admin@rucen.me",
            NormalizedEmail = "ADMIN@RUCEN.ME",
            EmailConfirmed = true,
            PasswordHash = "AQAAAAEAACcQAAAAEAzK2mB/YP6hSw5/c3BRuPSGhRlPFTp3arEGy9CGpYWSZ8XJju1yweq/2+PFzGVm6Q==",
            SecurityStamp = "SEJEPXC646ZBNCDYSM3H5FRK5RWP2TN6",
            ConcurrencyStamp = "b09a83ae-cfd3-4ee7-97e6-fbcf0b0fe78c",
            PhoneNumber = null,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnd = null,
            LockoutEnabled = true,
            AccessFailedCount = 0
        };

        return admin;
    }


    public ApplicationUser GetManager()
    {
        ApplicationUser manager = new ApplicationUser()
        {
            Id = "b3546cea-bc20-4f06-9341-15fb80768cf6",
            UserName = "manager@rucen.me",
            NormalizedUserName = "MANAGER@RUCEN.ME",
            Email = "manager@rucen.me",
            NormalizedEmail = "MANAGER@RUCEN.ME",
            EmailConfirmed = true,
            PasswordHash = "AQAAAAEAACcQAAAAEOZ07qoUUL9pwMR8+7xXWsldAicD2CFC6ufJsMWSYzFu9J8P3XCL9/tCxVMAKqyuaA==",
            SecurityStamp = "MAJXOSATJKOEM4YFF32Y5G2XPR5OFEL6",
            ConcurrencyStamp = "7a8d96fd-5918-441b-b800-cbafa99de97b",
            PhoneNumber = null,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnd = null,
            LockoutEnabled = true,
            AccessFailedCount = 0
        };

        return manager;
    }
    
    public ApplicationUser GetDemoUser()
    {
        ApplicationUser demoUser = new ApplicationUser()
        {
            Id = "505aedf9-29e3-4728-a8f0-d295040dbaef",
            UserName = "demo@rucen.me",
            NormalizedUserName = "DEMO@RUCEN.ME",
            Email = "demo@rucen.me",
            NormalizedEmail = "DEMO@RUCEN.ME",
            EmailConfirmed = true,
            PasswordHash = "AQAAAAEAACcQAAAAEF6Y+gXuBs8BQjWlNGbt586Sj20CDUvlsbLiWv+dP/+HawSCt4i+oXRH7g9DWHX+RA==",
            SecurityStamp = "TEJXOSATJKOEB4YFF32Y5G2XPR5OFEL7",
            ConcurrencyStamp = "acc87749-389b-4b5f-980c-0f3a49a37041",
            PhoneNumber = null,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnd = null,
            LockoutEnabled = true,
            AccessFailedCount = 0
        };

        return demoUser;
    }
}