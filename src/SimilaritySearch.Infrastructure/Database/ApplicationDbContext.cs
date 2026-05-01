using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimilaritySearch.Infrastructure.Database.Seeding;
using SimilaritySearch.Infrastructure.Identity;
using SimilaritySearch.Domain.Entities;

namespace SimilaritySearch.Infrastructure.Database;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, string>
{
    public DbSet<Ad> Ads { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresExtension("vector");
        
        // map relationships
        
        modelBuilder.Entity<Ad>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // approximate index
        modelBuilder.Entity<Ad>()
            .HasIndex(i => i.DescriptionEmbedding)
            .HasMethod("hnsw")
            .HasOperators("vector_l2_ops")
            .HasStorageParameter("m", 16)
            .HasStorageParameter("ef_construction", 64);

        // SEEDING IDENTITY

        // Init roles
        var rolesInit = new RolesInit();
        modelBuilder.Entity<Role>().HasData(rolesInit.GetRolesAm());

        // init users
        var userInit = new UserInit();
        var admin = userInit.GetAdmin();
        var manager = userInit.GetManager();
        var demoUser = userInit.GetDemoUser();

        // add users to the table
        modelBuilder.Entity<ApplicationUser>().HasData(admin, manager, demoUser);

        // assign roles to users
        var userRolesInit = new UserRolesInit();
        var adminUserRoles = userRolesInit.GetRolesForAdmin();
        var managerUserRoles = userRolesInit.GetRolesForManager();
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(adminUserRoles);
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(managerUserRoles);
    }
}