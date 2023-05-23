using Microsoft.EntityFrameworkCore;

namespace Model.Cooperative
{
    public class CooperativeContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<StepProject> StepProjects { get; set; }
        public CooperativeContext(DbContextOptions options) :
            base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Employee>()
            .HasMany(e => e.Steps)
            .WithOne(sp => sp.Employee)
    ;

            modelBuilder.Entity<Project>().Property(p => p.ProjectBudget).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<Coop>().Property(p => p.Budget).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<Membre>().Property(p => p.FeesPerYear).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<Employee>().Property(p => p.Salary).HasColumnType("decimal(18,4)");
           
            modelBuilder.Entity<CoopManager>().Property(p => p.Salary).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<CoopManager>().Property(p => p.ProjectBudget).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<CoopManager>().Property(p => p.ExpenseBudget).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<CoopManager>().Property(p => p.AfterStepBudget).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<StepProject>().Property(p => p.StepBudget).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<StepProject>().Property(p => p.NbreOfDays).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<OfflineMember>().Property(p => p.FeesPerYear).HasColumnType("decimal(18,4)");
        }

        public DbSet<Coop> Coop { get; set; }
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