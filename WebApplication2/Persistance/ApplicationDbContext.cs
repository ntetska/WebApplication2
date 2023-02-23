using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Domain;
public class ApplicationDbContext :DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<User> User { get; set; }
    public DbSet<Vacation> Vacation { get; set; }   
    public DbSet<RegistrationRequest> Request { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasMany(v => v.Vacation).WithOne(v => v.Petitioner).IsRequired();
        modelBuilder.Entity<User>().HasOne(a => a.Manager).WithMany(a => a.ManagedUsers).IsRequired(false);
        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<User>().Property(u => u.Username).HasColumnType("nvarchar(255)");
        modelBuilder.Entity<User>().HasOne(a => a.Request).WithOne(a => a.Applicant).IsRequired(false).OnDelete(DeleteBehavior.Cascade);
    }

}