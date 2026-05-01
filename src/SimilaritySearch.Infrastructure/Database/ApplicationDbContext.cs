using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StockMapSvelte.Domain.Entities;
using StockMapSvelte.Infrastructure.Database.Seeding;
using StockMapSvelte.Infrastructure.Identity;

namespace StockMapSvelte.Infrastructure.Database;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, string>
{
    
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // map relationships
            
            
            // SEEDING ENTITIES
            

            // SEEDING IDENTITY

            // Init roles
            RolesInit rolesInit = new RolesInit();
            modelBuilder.Entity<Role>().HasData(rolesInit.GetRolesAm());

            // init users
            UserInit userInit = new UserInit();
            ApplicationUser admin = userInit.GetAdmin();
            ApplicationUser manager = userInit.GetManager();
            ApplicationUser demoUser = userInit.GetDemoUser();

            // add users to the table
            modelBuilder.Entity<ApplicationUser>().HasData(admin, manager, demoUser);

            // assign roles to users
            UserRolesInit userRolesInit = new UserRolesInit();
            List<IdentityUserRole<string>> adminUserRoles = userRolesInit.GetRolesForAdmin();
            List<IdentityUserRole<string>> managerUserRoles = userRolesInit.GetRolesForManager();
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(adminUserRoles);
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(managerUserRoles);
        }
}