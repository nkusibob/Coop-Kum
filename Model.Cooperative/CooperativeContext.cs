using Microsoft.EntityFrameworkCore;
using Model.Cooperative.Migrations;
using Model.Cooperative.Model;
using System.Collections.Generic;

namespace Model.Cooperative
{
    public class CooperativeContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<StepProject> StepProjects { get; set; }

        public DbSet<Model.StepCategorie> StepCategories { get; set; }
        public DbSet<LivestockImage> LivestockImages { get; set; }
        public DbSet<PersonPicture> PersonImages { get; set; }
        public DbSet<StepProjectPicture> StepProjectPicture { get; set; }
        public DbSet<SocialAssistance> SocialAssistances { get; set; }


        public CooperativeContext(DbContextOptions options) :
            base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<GoatPair>();
            modelBuilder.Entity<Livestock>()
               .Property(l => l.Weight)
               .HasColumnType("decimal(18, 2)");
           
            modelBuilder.Entity<LivestockImage>()
            .HasOne(i => i.Livestock)
            .WithMany(l => l.Images)
            .HasForeignKey(i => i.LivestockId)

            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Employee>()
            .HasMany(e => e.Steps)
            .WithOne(sp => sp.Employee);

            modelBuilder.Entity<SocialAssistance>(entity =>
            {
                entity.HasKey(x => x.SocialAssistId);

                entity.Property(x => x.Amount)
                      .HasColumnType("decimal(18,4)");

                entity.HasOne(x => x.Membre)
                      .WithMany() // or .WithMany(m => m.SocialAssistances)
                      .HasForeignKey(x => x.MembreId)
                      .OnDelete(DeleteBehavior.Restrict);

                // 🔒 Business rule: only ONE active (not repaid) aid per member
                entity.HasIndex(x => x.MembreId)
                      .IsUnique()
                      .HasFilter("[IsRepaid] = 0");
            });



            modelBuilder.Entity<Livestock>()
                .HasDiscriminator<string>("LivestockType")
                .HasValue<Goat>("Goat");

            modelBuilder.Entity<Livestock>()
                .HasOne(l => l.Mother)
                .WithMany(l => l.Kids)
                .HasForeignKey(l => l.MotherId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Goat>()
                .HasOne(g => g.Father)
                .WithMany()
                .HasForeignKey(g => g.FatherId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Livestock>()
               .Property(l => l.LivestockType)
               .HasConversion<string>();

            modelBuilder.Entity<Project>().Property(p => p.ProjectBudget).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<Coop>().Property(p => p.Budget).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<Membre>().Property(p => p.FeesPerYear).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<Employee>().Property(p => p.DailySalary).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<Livestock>().Property(p => p.Price).HasColumnType("decimal(18,4)");

            modelBuilder.Entity<CoopManager>().Property(p => p.ManagerSalary).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<CoopManager>().Property(p => p.ProjectBudget).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<CoopManager>().Property(p => p.ExpenseBudget).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<CoopManager>().Property(p => p.AfterStepBudget).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<StepProject>().Property(p => p.StepBudget).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<StepProject>().Property(p => p.NbreOfDays).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<OfflineMember>().Property(p => p.FeesPerYear).HasColumnType("decimal(18,4)");
        }

        public DbSet<Coop> Coop { get; set; }

        public DbSet<Livestock> Livestock { get; set; }
        public DbSet<Goat> Goat { get; set; }

        public DbSet<GoatPair> GoatPairs { get; set; }

        public DbSet<Membre> Membre { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<StepProject> StepProject { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<CoopManager> Manager { get; set; }
        public DbSet<ConnectedMember> ConnectedMember { get; set; }
        public DbSet<OfflineMember> OfflineMember { get; set; }
        public DbSet<Person> Person { get; set; }
    }
}