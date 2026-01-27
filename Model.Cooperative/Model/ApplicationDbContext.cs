using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model.Cooperative.Model;

namespace Model.Cooperative
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationUserImage> ApplicationUserImages { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUserImage>(entity =>
            {
                entity.HasKey(e => e.ApplicationUserImageId);

                entity.Property(e => e.Data).IsRequired();

                entity.HasOne(e => e.AspUser)
                      .WithOne(u => u.UserImage)
                      .HasForeignKey<ApplicationUserImage>(e => e.AspUserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.AspUserId).IsUnique(); // 1 image max per user
            });


        }
    }
}