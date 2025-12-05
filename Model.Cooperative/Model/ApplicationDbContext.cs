using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model.Cooperative.Model;

namespace Model.Cooperative
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
      

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUserImage>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Define the foreign key to ApplicationUser
                entity.HasOne(e => e.AspUser)
                      .WithOne(u => u.UserImage)
                      .HasForeignKey<ApplicationUserImage>(e => e.AspUserId)
                      .IsRequired(); // If the user image is required, otherwise, remove this line.

                // Other configurations for ApplicationUserImage...
            });

        }
    }
}